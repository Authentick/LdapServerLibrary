using System.Collections.Generic;
using System.Threading.Tasks;
using LdapServer.Models.Operations;
using LdapServer.Models.Operations.Request;
using LdapServer.Models.Operations.Response;
using LdapServer.Session.Events;
using LdapServer.Session.Replies;

namespace LdapServer.Engine.Handler
{
    internal class SearchRequestHandler : IRequestHandler<SearchRequest>
    {
        async Task<HandlerReply> IRequestHandler<SearchRequest>.Handle(ClientContext context, LdapEvents eventListener, SearchRequest operation)
        {
            SearchEvent searchEvent = new SearchEvent();
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
