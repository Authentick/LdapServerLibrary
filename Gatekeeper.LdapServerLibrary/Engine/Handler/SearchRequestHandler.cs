using System.Collections.Generic;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary.Models.Operations;
using Gatekeeper.LdapServerLibrary.Models.Operations.Request;
using Gatekeeper.LdapServerLibrary.Models.Operations.Response;
using Gatekeeper.LdapServerLibrary.Session.Events;
using Gatekeeper.LdapServerLibrary.Session.Replies;

namespace Gatekeeper.LdapServerLibrary.Engine.Handler
{
    internal class SearchRequestHandler : IRequestHandler<SearchRequest>
    {
        async Task<HandlerReply> IRequestHandler<SearchRequest>.Handle(ClientContext context, LdapEvents eventListener, SearchRequest operation)
        {
            SearchEvent searchEvent = new SearchEvent{
                Filter = operation.Filter,
            };
            List<SearchResultReply> replies = await eventListener.OnSearchRequest(new ClientContext(), searchEvent);

            List<IProtocolOp> opReply = new List<IProtocolOp>();

            foreach (SearchResultReply reply in replies)
            {
                SearchResultEntry entry = new SearchResultEntry(reply);
                opReply.Add(entry);
            }

            LdapResult ldapResult = new LdapResult(LdapResult.ResultCodeEnum.Success, null, null);
            SearchResultDone searchResultDone = new SearchResultDone(ldapResult);
            opReply.Add(searchResultDone);

            return new HandlerReply(opReply);
        }
    }
}
