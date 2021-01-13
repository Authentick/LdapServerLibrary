using Gatekeeper.LdapServerLibrary.PacketParser.Models.Operations;

namespace Gatekeeper.LdapServerLibrary.PacketParser.Models.Operations.Request
{
    internal class UnbindRequest : IProtocolOp
    {
        internal const int Tag = 2;
    }
}
