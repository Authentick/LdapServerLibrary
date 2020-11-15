using System.Collections.Generic;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary.Models.Operations;
using Gatekeeper.LdapServerLibrary.Models.Operations.Request;
using Gatekeeper.LdapServerLibrary.Models.Operations.Response;
using Gatekeeper.LdapServerLibrary.Session.Events;

namespace Gatekeeper.LdapServerLibrary.Engine.Handler
{
    internal class BindRequestHandler : IRequestHandler<BindRequest>
    {
        async Task<HandlerReply> IRequestHandler<BindRequest>.Handle(ClientContext context, LdapEvents eventListener, BindRequest operation)
        {
            AuthenticationEvent authEvent = new AuthenticationEvent(operation.Name, operation.Authentication);
            bool success = await eventListener.OnAuthenticationRequest(new ClientContext(), authEvent);

            if (success)
            {
                context.IsAuthenticated = true;
                context.UserId = operation.Name;

                LdapResult ldapResult = new LdapResult(LdapResult.ResultCodeEnum.Success, null, null);
                BindResponse bindResponse = new BindResponse(ldapResult);
                return new HandlerReply(new List<IProtocolOp> { bindResponse });
            }
            else
            {
                context.IsAuthenticated = false;
                context.UserId = null;

                LdapResult ldapResult = new LdapResult(LdapResult.ResultCodeEnum.InappropriateAuthentication, null, null);
                BindResponse bindResponse = new BindResponse(ldapResult);
                return new HandlerReply(new List<IProtocolOp> { bindResponse });
            }
        }
    }
}
