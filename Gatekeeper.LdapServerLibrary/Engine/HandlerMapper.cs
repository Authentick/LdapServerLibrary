using System;
using Gatekeeper.LdapServerLibrary.Engine.Handler;
using Gatekeeper.LdapPacketParserLibrary.Models.Operations.Request;

namespace Gatekeeper.LdapServerLibrary.Engine
{
    internal class HandlerMapper
    {
        internal Type GetHandlerForType(Type type)
        {
            if (type == typeof(BindRequest))
            {
                return typeof(BindRequestHandler);
            }

            if (type == typeof(ExtendedRequest))
            {
                return typeof(ExtendedRequestHandler);
            }

            if (type == typeof(SearchRequest))
            {
                return typeof(SearchRequestHandler);
            }

            if (type == typeof(UnbindRequest))
            {
                return typeof(UnbindRequestHandler);
            }

            throw new NotImplementedException("Type " + type + " is not implemented");
        }
    }
}