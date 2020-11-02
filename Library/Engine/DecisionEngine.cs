using LdapServer.Models;
using LdapServer.Models.Operations.Request;
using LdapServer.Models.Operations.Response;

namespace LdapServer.Engine
{
    internal class DecisionEngine
    {
        internal LdapMessage GenerateReply(LdapMessage message)
        {
            LdapEvents eventListener = SingletonContainer.GetLdapEventListener();

            if (message.ProtocolOp.GetType() == typeof(BindRequest))
            {
                LdapResult ldapResult = new LdapResult(LdapResult.ResultCodeEnum.InappropriateAuthentication, null, null);
                BindResponse bindResponse = new BindResponse(ldapResult);
                LdapMessage outMessage = new LdapMessage(1, bindResponse);
                return outMessage;
            }
            throw new System.Exception();
        }
    }
}

