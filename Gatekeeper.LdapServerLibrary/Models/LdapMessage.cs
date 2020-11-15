using System.Numerics;
using LdapServer.Models.Operations;

namespace LdapServer.Models
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