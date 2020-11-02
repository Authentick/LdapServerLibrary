using LdapServer;
using LdapServer.Session.Events;

namespace Sample {
    class LdapEventListener : LdapEvents
    {
        public override bool OnAuthenticationRequest(ClientContext context, AuthenticationEvent authenticationEvent) {
            if(authenticationEvent.Username == "cn=Manager,dc=ldap,dc=net" && authenticationEvent.Password == "test") {
                return true;
            }

            return false;
        }
    }
}