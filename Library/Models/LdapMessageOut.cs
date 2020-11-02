using System.Numerics;
using LdapServer.Models.Operations.Response;

namespace LdapServer.Models
{
    internal class LdapMessageOut
    {
        internal readonly BigInteger MessageId;
        internal readonly IResponse Response;

        internal LdapMessageOut(
            BigInteger messageId,
            IResponse response
            )
        {
            MessageId = messageId;
            Response = response;
        }
    }
}