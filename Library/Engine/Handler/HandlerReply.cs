using System.Collections.Generic;
using LdapServer.Models.Operations;

namespace LdapServer.Engine.Handler
{
    internal class HandlerReply
    {
        internal readonly List<IProtocolOp> _protocolOps;

        public HandlerReply(List<IProtocolOp> protocolOps)
        {
            _protocolOps = protocolOps;
        }
    }
}
