using System.Collections.Generic;
using Gatekeeper.LdapServerLibrary.Models.Operations;

namespace Gatekeeper.LdapServerLibrary.Engine.Handler
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
