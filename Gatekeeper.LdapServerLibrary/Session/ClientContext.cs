using System.Collections.Generic;
using System.Net;

namespace Gatekeeper.LdapServerLibrary
{
    public class ClientContext
    {
        public bool IsAuthenticated { get; set; }
        public bool HasEncryptedConnection { get; set; }
        public Dictionary<string, List<string>> Rdn { get; set; } = new Dictionary<string, List<string>>();
        public readonly IPAddress IpAddress;

        public ClientContext(IPAddress ipAddress)
        {
            IpAddress = ipAddress;
        }
    }
}
