using Gatekeeper.LdapServerLibrary.Session.Replies;

namespace Gatekeeper.LdapServerLibrary.Models.Operations.Response
{
    internal class SearchResultEntry : IProtocolOp
    {
        internal readonly SearchResultReply SearchResultReply;

        internal SearchResultEntry(
            SearchResultReply searchResultReply
        )
        {
            SearchResultReply = searchResultReply;
        }

        int IProtocolOp.GetTag()
        {
            return 4;
        }
    }
}