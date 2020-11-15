using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary;

namespace Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
          LdapServer server = new LdapServer();
          server.RegisterEventListener(new LdapEventListener());
          await server.Start();
        }
    }
}
