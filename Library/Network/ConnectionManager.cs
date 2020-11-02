using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace LdapServer.Network
{
    internal class ConnectionManager
    {
        private Dictionary<Guid, ClientSession> _clients = new Dictionary<Guid, ClientSession>();

        internal void AddClient(TcpClient client)
        {
            Guid id = Guid.NewGuid();
            ClientSession session = new ClientSession(id, client);
            _clients.Add(id, session);
            session.StartReceiving();
        }
    }
}
