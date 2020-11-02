using LdapServer.Models.Operations;

namespace LdapServer.Engine.Handler
{
    internal class HandlerReply
    {
        internal readonly IProtocolOp _protocolOp;

        public HandlerReply(IProtocolOp protocolOp)
        {
            _protocolOp = protocolOp;
        }
    }
}
