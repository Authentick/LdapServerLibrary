using System.Collections.Generic;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary.Models.Operations;
using Gatekeeper.LdapServerLibrary.Models.Operations.Response;
using Gatekeeper.LdapPacketParserLibrary.Models.Operations.Request;

namespace Gatekeeper.LdapServerLibrary.Engine.Handler
{
    internal class ExtendedRequestHandler : IRequestHandler<ExtendedRequest>
    {
        internal const string StartTLS = "1.3.6.1.4.1.1466.20037";

        async Task<HandlerReply> IRequestHandler<ExtendedRequest>.Handle(ClientContext context, LdapEvents eventListener, ExtendedRequest operation)
        {
            if (operation.RequestName == StartTLS && SingletonContainer.GetCertificate() != null)
            {
                context.HasEncryptedConnection = true;
                return new HandlerReply(new List<IProtocolOp>{
                    new ExtendedOperationResponse(
                        new LdapResult(LdapResult.ResultCodeEnum.Success, null, null),
                        StartTLS,
                        null
                    ),
                });
            }

            LdapResult ldapResult = new LdapResult(LdapResult.ResultCodeEnum.ProtocolError, null, null);
            BindResponse bindResponse = new BindResponse(ldapResult);
            return new HandlerReply(new List<IProtocolOp> { bindResponse });
        }
    }
}
