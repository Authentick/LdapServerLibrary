using System;
using System.Net;
using System.Net.Sockets;
using System.Threading.Tasks;

namespace Gatekeeper.LdapServerLibrary.Network
{
    internal class NetworkListener
    {
        private readonly ConnectionManager _connectionManager;
        private readonly int _port;
        private readonly IPAddress _ipAddress;

        internal NetworkListener(
            ConnectionManager connectionManager, 
            IPAddress ipAddress,
            int port)
        {
            _connectionManager = connectionManager;
            _ipAddress = ipAddress;
            _port = port;
        }

        internal async Task Start()
        {
            TcpListener? server = null;
            try
            {
                server = new TcpListener(_ipAddress, _port);
                server.Start();

                while (true)
                {
                    TcpClient client = await server.AcceptTcpClientAsync();
                    _connectionManager.AddClient(client);
                }
            }
            finally
            {
                if (server != null)
                {
                    server.Stop();
                }
            }
        }
    }
}
