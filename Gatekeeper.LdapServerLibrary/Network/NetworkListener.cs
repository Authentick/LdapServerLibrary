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

        internal NetworkListener(ConnectionManager connectionManager, int port)
        {
            _connectionManager = connectionManager;
            _port = port;
        }

        internal async Task Start()
        {
            TcpListener? server = null;
            try
            {
                Int32 port = _port;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                server = new TcpListener(localAddr, port);
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
