using System.Collections.Generic;
using LdapServer.Models.Operations;
using LdapServer.Models.Operations.Request;
using LdapServer.Models.Operations.Response;
using LdapServer.Session.Events;

namespace LdapServer.Engine.Handler
{
    internal class SearchRequestHandler : IRequestHandler<SearchRequest>
    {
        HandlerReply IRequestHandler<SearchRequest>.Handle(ClientContext context, LdapEvents eventListener, SearchRequest operation)
        {
            LdapResult ldapResult = new LdapResult(LdapResult.ResultCodeEnum.Success, null, null);
            SearchResultDone searchResultDone = new SearchResultDone(ldapResult);
            return new HandlerReply(new List<IProtocolOp>{new SearchResultEntry(), searchResultDone});
        }
    }
}
