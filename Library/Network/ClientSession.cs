using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using LdapServer.Engine;
using LdapServer.Models;
using LdapServer.Models.Operations.Request;
using LdapServer.Models.Operations.Response;
using LdapServer.Parser;

namespace LdapServer.Network
{
    internal class ClientSession
    {
        internal readonly Guid Id;
        internal readonly TcpClient Client;

        internal ClientSession(Guid id, TcpClient client)
        {
            Id = id;
            Client = client;
        }

        internal void StartReceiving()
        {
            Task networkTask = new Task(() =>
            {
                // Buffer for reading data
                Byte[] bytes = new Byte[2048];

                // Get a stream object for reading and writing
                NetworkStream stream = Client.GetStream();

                int i;

                DecisionEngine engine = new DecisionEngine(new ClientContext());

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    PacketParser parser = new PacketParser();
                    LdapMessage message = parser.TryParsePacket(bytes);

                    LdapMessage reply = engine.GenerateReply(message);
                    
                    byte[] msg = parser.TryEncodePacket(reply);
                    stream.Write(msg, 0, msg.Length);
                }
            });

            networkTask.Start();

        }
    }
}