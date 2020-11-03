using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using LdapServer.Engine.Handler;
using LdapServer.Models;
using LdapServer.Models.Operations;
using LdapServer.Models.Operations.Request;

namespace LdapServer.Engine
{
    internal class DecisionEngine
    {
        private readonly ClientContext _clientContext;

        public DecisionEngine(ClientContext clientContext)
        {
            _clientContext = clientContext;
        }

        internal async Task<List<LdapMessage>> GenerateReply(LdapMessage message)
        {
            // Require authentication for everything that isn't a bind
            if (!_clientContext.IsAuthenticated && message.ProtocolOp.GetType() != typeof(BindRequest))
            {
                throw new Exception("User is not authenticated");
            }

            LdapEvents eventListener = SingletonContainer.GetLdapEventListener();

            Type protocolType = message.ProtocolOp.GetType();
            Type handlerType = SingletonContainer.GetHandlerMapper().GetHandlerForType(protocolType);

            var parameters = new object[] { _clientContext, eventListener, message.ProtocolOp };
            object? invokableClass = FormatterServices.GetUninitializedObject(handlerType);

            if (invokableClass != null)
            {
                MethodInfo? method = handlerType.GetMethods(BindingFlags.NonPublic | BindingFlags.Instance).Single(x => x.Name.EndsWith("Handle"));

                if (method != null)
                {
                    Task resultTask = (Task)method.Invoke(invokableClass, parameters);
                    await resultTask;
                    PropertyInfo? propertInfo = resultTask.GetType().GetProperty("Result");
                    object  result = propertInfo.GetValue(resultTask);

                    if (result != null)
                    {
                        List<LdapMessage> messages = new List<LdapMessage>();

                        HandlerReply handlerReply = (HandlerReply)result;
                        foreach (IProtocolOp op in handlerReply._protocolOps)
                        {
                            messages.Add(new LdapMessage(message.MessageId, op));
                        }

                        return messages;
                    }
                }
            }

            throw new System.Exception();
        }
    }
}

