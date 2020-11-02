using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using LdapServer.Engine.Handler;
using LdapServer.Models;

namespace LdapServer.Engine
{
    internal class DecisionEngine
    {
        private readonly ClientContext _clientContext;

        public DecisionEngine(ClientContext clientContext)
        {
            _clientContext = clientContext;
        }

        internal LdapMessage GenerateReply(LdapMessage message)
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
                        HandlerReply handlerReply = (HandlerReply) result;
                        return new LdapMessage(1, handlerReply._protocolOp);
                    }
                }
            }

            throw new System.Exception();
        }
    }
}

