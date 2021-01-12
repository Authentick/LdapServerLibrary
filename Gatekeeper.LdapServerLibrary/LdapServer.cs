using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary.Network;

namespace Gatekeeper.LdapServerLibrary
{
    public class LdapServer
    {
        public int Port = 339;

        public void RegisterEventListener(LdapEvents ldapEvents)
        {
            SingletonContainer.SetLdapEventListener(ldapEvents);
        }

        public void RegisterLogger(ILogger logger)
        {
            SingletonContainer.SetLogger(logger);
        }

        public void RegisterCertificate(X509Certificate2 certificate)
        {
            SingletonContainer.SetCertificate(certificate);
        }

        public async Task Start()
        {
            ConnectionManager manager = new ConnectionManager();
            NetworkListener listener = new NetworkListener(manager, Port);
            await listener.Start();
        }
    }
}
