using System.Collections.Generic;
using System.Threading.Tasks;
using LdapServer.Models.Operations;
using LdapServer.Models.Operations.Request;

namespace LdapServer.Engine.Handler
{
    internal class UnbindRequestHandler : IRequestHandler<UnbindRequest>
    {
        Task<HandlerReply> IRequestHandler<UnbindRequest>.Handle(ClientContext context, LdapEvents eventListener, UnbindRequest operation)
        {
            context.IsAuthenticated = false;
            context.UserId = null;
            return Task.FromResult(new HandlerReply(new List<IProtocolOp> { }));
        }
    }
}
