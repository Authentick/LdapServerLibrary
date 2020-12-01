using System.Collections.Generic;

namespace Gatekeeper.LdapServerLibrary.Session.Events
{
    public interface IAuthenticationEvent
    {
        public Dictionary<string, List<string>> Rdn { get; }
        public string Password { get; }
    }
}
