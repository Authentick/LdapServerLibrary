using LdapServer.Models.Operations.Request;
using LdapServer.Models.Operations.Response;
using LdapServer.Session.Events;

namespace LdapServer.Engine.Handler
{
    internal class SearchRequestHandler : IRequestHandler<SearchRequest>
    {
        HandlerReply IRequestHandler<SearchRequest>.Handle(ClientContext context, LdapEvents eventListener, SearchRequest operation)
        {
            System.Console.WriteLine("Search Request");
            System.Console.WriteLine(context.UserId);
            throw new System.Exception("Not implemented");
        }
    }
}
