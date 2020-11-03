using LdapServer.Session.Replies;

namespace LdapServer.Models.Operations.Response
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