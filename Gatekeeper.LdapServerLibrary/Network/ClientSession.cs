using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary.Engine;
using Gatekeeper.LdapServerLibrary.Models;
using Gatekeeper.LdapServerLibrary.Parser;

namespace Gatekeeper.LdapServerLibrary.Network
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
            Task networkTask = new Task(async () =>
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

                    List<LdapMessage> replies = await engine.GenerateReply(message);
                    foreach (LdapMessage outMsg in replies)
                    {
                        byte[] msg = parser.TryEncodePacket(outMsg);
                        stream.Write(msg, 0, msg.Length);
                    }
                }
            });

            networkTask.Start();

        }
    }
}