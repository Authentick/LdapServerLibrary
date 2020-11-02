using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using LdapServer.Engine.Handler;
using LdapServer.Models;
using LdapServer.Models.Operations;

namespace LdapServer.Engine
{
    internal class DecisionEngine
    {
        private readonly ClientContext _clientContext;

        public DecisionEngine(ClientContext clientContext)
        {
            _clientContext = clientContext;
        }

        internal List<LdapMessage> GenerateReply(LdapMessage message)
        {
            LdapEvents eventListener = SingletonContainer.GetLdapEventListener();

            Type protocolType = message.ProtocolOp.GetType();
            Type handlerType = SingletonContainer.GetHandlerMapper().GetHandlerForType(protocolType);

            var parameters = new object[] { _clientContext, eventListener, message.ProtocolOp};
            object? invokableClass = FormatterServices.GetUninitializedObject(handlerType);

            if (invokableClass != null)
            {
                MethodInfo? method = handlerType.GetMethods(BindingFlags.NonPublic|BindingFlags.Instance).Single(x => x.Name.EndsWith("Handle"));

                if (method != null)
                {
                    object result = method.Invoke(invokableClass, parameters);
                    if (result != null)
                    {
                        List<LdapMessage> messages = new List<LdapMessage>();

                        HandlerReply handlerReply = (HandlerReply) result;
                        foreach (IProtocolOp op in handlerReply._protocolOps) {
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

