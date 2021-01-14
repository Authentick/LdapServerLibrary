using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary.Engine.Handler;
using Gatekeeper.LdapServerLibrary.Models;
using Gatekeeper.LdapServerLibrary.Models.Operations;
using Gatekeeper.LdapServerLibrary.PacketParser.Models.Operations.Request;
using Gatekeeper.LdapServerLibrary.Models.Operations.Response;

namespace Gatekeeper.LdapServerLibrary.Engine
{
    internal class DecisionEngine
    {
        private readonly ClientContext _clientContext;

        public DecisionEngine(ClientContext clientContext)
        {
            _clientContext = clientContext;
        }

        internal async Task<List<LdapMessage>> GenerateReply(PacketParser.Models.LdapMessage message)
        {
            // Authentication check
            List<Type> publicOperations = new List<Type>{
                typeof(BindRequest),
                typeof(UnbindRequest),
                typeof(ExtendedRequest),
            };
            if (!_clientContext.IsAuthenticated && !publicOperations.Contains(message.ProtocolOp.GetType()))
            {
                return new List<LdapMessage>(){
                    new LdapMessage(message.MessageId, new BindResponse(new LdapResult(LdapResult.ResultCodeEnum.InappropriateAuthentication, null, null)))
                };
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
                    object result = propertInfo.GetValue(resultTask);

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
