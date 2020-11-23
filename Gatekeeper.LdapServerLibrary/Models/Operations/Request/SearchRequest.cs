using static Gatekeeper.LdapServerLibrary.Session.Events.SearchEvent;

namespace Gatekeeper.LdapServerLibrary.Models.Operations.Request
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

        internal IFilterChoice Filter = null!;
        internal string BaseObject = null!;

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
