using System.Net.Sockets;

namespace Gatekeeper.LdapServerLibrary.Network
{
    internal class ConnectionManager
    {
        internal void AddClient(TcpClient client)
        {
            ClientSession session = new ClientSession(client);
            session.StartReceiving();
        }
    }
}
