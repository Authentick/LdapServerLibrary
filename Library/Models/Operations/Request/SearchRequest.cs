using System.Formats.Asn1;
using System.Numerics;

namespace LdapServer.Models.Operations.Request
{
    internal class SearchRequest : IProtocolOp
    {

        internal SearchRequest()
        {
        }

        int IProtocolOp.GetTag()
        {
            return 3;
        }
    }
}
