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

        internal enum ScopeEnum
        {
            BaseObject = 0,
            SingleLevel = 1,
            WholeSubtree = 2,
        }

        internal enum DerefAliasesEnum
        {
            NeverDerefAliases = 0,
            DerefInSearching = 1,
            DerefFindingBaseObj = 2,
            DerefAlways = 3,
        }
    }
}
