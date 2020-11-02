using LdapServer.Models.Operations.Request;
using LdapServer.Models.Operations.Response;
using LdapServer.Session.Events;

namespace LdapServer.Engine.Handler
{
    internal class BindRequestHandler : IRequestHandler<BindRequest>
    {
        HandlerReply IRequestHandler<BindRequest>.Handle(ClientContext context, LdapEvents eventListener, BindRequest operation)
        {
            AuthenticationEvent authEvent = new AuthenticationEvent(operation.Name, operation.Authentication);
            bool success = eventListener.OnAuthenticationRequest(new ClientContext(), authEvent);

            if (success)
            {
                context.IsAuthenticated = true;
                context.UserId = operation.Name;

                LdapResult ldapResult = new LdapResult(LdapResult.ResultCodeEnum.Success, null, null);
                BindResponse bindResponse = new BindResponse(ldapResult);
                return new HandlerReply(bindResponse);
            }
            else
            {
                context.IsAuthenticated = false;
                context.UserId = null;

                LdapResult ldapResult = new LdapResult(LdapResult.ResultCodeEnum.InappropriateAuthentication, null, null);
                BindResponse bindResponse = new BindResponse(ldapResult);
                return new HandlerReply(bindResponse);
            }
        }
    }
}
