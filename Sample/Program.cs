using System.Security.Cryptography.X509Certificates;
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
            server.RegisterLogger(new ConsoleLogger());
            server.RegisterCertificate(new X509Certificate2("/root/workspace/LdapServerLibrary/Sample/example_certificate.pfx"));
            await server.Start();
        }
    }
}
