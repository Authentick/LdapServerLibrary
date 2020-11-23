using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary;

namespace Sample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            LdapServer server = new LdapServer
            {
                Port = 3389,
            };
            server.RegisterEventListener(new LdapEventListener());
            await server.Start();
        }
    }
}
