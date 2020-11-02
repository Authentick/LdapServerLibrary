using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;
using LdapServer.Models;
using LdapServer.Models.Operations.Response;
using LdapServer.Network;

namespace LdapServer
{
    public class LdapServer
    {
        private LdapEvents LdapEventListener = new LdapEvents();

        public void RegisterEventListener(LdapEvents ldapEvents) {
            LdapEventListener = ldapEvents;
        }

        public async Task Start()
        {
           ConnectionManager manager = new ConnectionManager(); 
           NetworkListener listener= new NetworkListener(manager);
           await listener.Start();
        }
    }
}