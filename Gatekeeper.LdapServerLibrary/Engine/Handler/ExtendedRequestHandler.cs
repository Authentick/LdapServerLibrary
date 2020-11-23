using System.Collections.Generic;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary.Models.Operations;
using Gatekeeper.LdapServerLibrary.Models.Operations.Request;
using Gatekeeper.LdapServerLibrary.Models.Operations.Response;
using Gatekeeper.LdapServerLibrary.Session.Events;

namespace Gatekeeper.LdapServerLibrary.Engine.Handler
{
    internal class ExtendedRequestHandler : IRequestHandler<ExtendedRequest>
    {
        async Task<HandlerReply> IRequestHandler<ExtendedRequest>.Handle(ClientContext context, LdapEvents eventListener, ExtendedRequest operation)
        {
          
            LdapResult ldapResult = new LdapResult(LdapResult.ResultCodeEnum.ProtocolError, null, null);
            BindResponse bindResponse = new BindResponse(ldapResult);
            return new HandlerReply(new List<IProtocolOp> { bindResponse });
        }
    }
}
