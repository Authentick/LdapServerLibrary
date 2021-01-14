using System.Collections.Generic;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary.Models.Operations;
using Gatekeeper.LdapServerLibrary.Models.Operations.Response;
using Gatekeeper.LdapPacketParserLibrary.Models.Operations.Request;
using Gatekeeper.LdapServerLibrary.Session.Events;
using Gatekeeper.LdapServerLibrary.Session.Replies;

namespace Gatekeeper.LdapServerLibrary.Engine.Handler
{
    internal class SearchRequestHandler : IRequestHandler<SearchRequest>
    {
        async Task<HandlerReply> IRequestHandler<SearchRequest>.Handle(ClientContext context, LdapEvents eventListener, SearchRequest operation)
        {
            SearchEvent searchEvent = new SearchEvent
            {
                SearchRequest = operation,
            };
            List<SearchResultReply> replies = await eventListener.OnSearchRequest(context, searchEvent);

            List<IProtocolOp> opReply = new List<IProtocolOp>();

            foreach (SearchResultReply reply in replies)
            {
                SearchResultEntry entry = new SearchResultEntry(reply);
                opReply.Add(entry);
            }

            var resultCode = (replies.Count > 0) ? LdapResult.ResultCodeEnum.Success : LdapResult.ResultCodeEnum.NoSuchObject;

            LdapResult ldapResult = new LdapResult(resultCode, null, null);
            SearchResultDone searchResultDone = new SearchResultDone(ldapResult);
            opReply.Add(searchResultDone);

            return new HandlerReply(opReply);
        }
    }
}
