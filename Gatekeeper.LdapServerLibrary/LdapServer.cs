using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary.Network;

namespace Gatekeeper.LdapServerLibrary
{
    public class LdapServer
    {
        public void RegisterEventListener(LdapEvents ldapEvents) {
            SingletonContainer.SetLdapEventListener(ldapEvents);
        }

        public async Task Start()
        {
           ConnectionManager manager = new ConnectionManager(); 
           NetworkListener listener= new NetworkListener(manager);
           await listener.Start();
        }
    }
}