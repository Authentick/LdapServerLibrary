using System.Threading.Tasks;

namespace Sample
{
    class Program
    {
        static async Task Main(string[] args)
        {
          LdapServer.LdapServer server = new LdapServer.LdapServer();
          server.RegisterEventListener(new LdapEventListener());
          await server.Start();
        }
    }
}
