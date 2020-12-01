using System.Collections.Generic;

namespace Gatekeeper.LdapServerLibrary
{
    public class ClientContext
    {
        public bool IsAuthenticated { get; set; }
        public Dictionary<string, List<string>> Rdn { get; set; } = new Dictionary<string, List<string>>();
    }
}
