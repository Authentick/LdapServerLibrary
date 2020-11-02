using System.Threading.Tasks;
using LdapServer.Models.Operations.Request;
using LdapServer.Models.Operations.Response;
using LdapServer.Session.Events;

namespace LdapServer
{
    public class LdapEvents
    {
        /// <summary>
        /// Override this for authentication requests.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="authenticationEvent"></param>
        /// <returns>Whether the authentication should succeed or not</returns>
        public virtual bool OnAuthenticationRequest(ClientContext context, AuthenticationEvent authenticationEvent) {
            return false;
        }
    }
}