using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using LdapServer;

namespace Sample
{
    class Program
    {
        static void Main(string[] args)
        {
          LdapServer.LdapServer server = new LdapServer.LdapServer();
          server.Start();
        }
    }
}
