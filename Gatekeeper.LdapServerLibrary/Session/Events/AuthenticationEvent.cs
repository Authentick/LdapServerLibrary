using System.Collections.Generic;

namespace Gatekeeper.LdapServerLibrary.Session.Events
{
    internal class AuthenticationEvent : IAuthenticationEvent
    {
        public Dictionary<string, List<string>> Rdn { get; }
        public string Password { get; }

        public AuthenticationEvent(
            Dictionary<string, List<string>> rdn,
            string password
            )
        {
            Rdn = rdn;
            Password = password;
        }
    }
}
