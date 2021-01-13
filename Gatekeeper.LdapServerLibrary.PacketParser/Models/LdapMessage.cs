using System.Numerics;
using Gatekeeper.LdapServerLibrary.PacketParser.Models.Operations;

namespace Gatekeeper.LdapServerLibrary.PacketParser.Models
{
    public class LdapMessage
    {
        public readonly BigInteger MessageId;
        public readonly IProtocolOp ProtocolOp;

        internal LdapMessage(
            BigInteger messageId,
            IProtocolOp protocolOp
            )
        {
            MessageId = messageId;
            ProtocolOp = protocolOp;
        }
    }
}