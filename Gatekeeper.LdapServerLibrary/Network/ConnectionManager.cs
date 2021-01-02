using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Gatekeeper.LdapServerLibrary.Network
{
    internal class ConnectionManager
    {
        private List<ClientSession> _clients = new List<ClientSession>();

        internal void AddClient(TcpClient client)
        {
            Guid id = Guid.NewGuid();
            ClientSession session = new ClientSession(client);
            _clients.Add(session);
            session.StartReceiving();
        }
    }
}
