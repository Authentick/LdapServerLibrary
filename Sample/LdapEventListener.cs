using LdapServer;
using LdapServer.Session.Events;

namespace Sample {
    class LdapEventListener : LdapEvents
    {
        public override bool OnAuthenticationRequest(ClientContext context, AuthenticationEvent authenticationEvent) {
            return true;
        }
    }
}