using System.Numerics;
using LdapServer.Models.Operations.Request;

namespace LdapServer.Models
{
    internal class LdapMessage
    {
        internal readonly BigInteger MessageId;
        internal readonly IRequest Request;

        internal LdapMessage(
            BigInteger messageId,
            IRequest request
            )
        {
            MessageId = messageId;
            Request = request;
        }
    }
}