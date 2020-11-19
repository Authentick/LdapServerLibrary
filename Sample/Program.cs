using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary;

namespace Sample
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
          LdapServer server = new LdapServer();
          server.RegisterEventListener(new LdapEventListener());
          await server.Start();
        }
    }
}
