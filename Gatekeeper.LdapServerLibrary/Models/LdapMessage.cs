using System.Numerics;
using Gatekeeper.LdapServerLibrary.Models.Operations;

namespace Gatekeeper.LdapServerLibrary.Models
{
    internal class LdapMessage
    {
        internal readonly BigInteger MessageId;
        internal readonly IProtocolOp ProtocolOp;

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