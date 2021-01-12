using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary.Engine;
using Gatekeeper.LdapServerLibrary.Models;
using Gatekeeper.LdapServerLibrary.Parser;
using Gatekeeper.LdapServerLibrary.Engine.Handler;

namespace Gatekeeper.LdapServerLibrary.Network
{
    internal class ClientSession
    {
        internal readonly TcpClient Client;
        private bool _useStartTls;
        private bool _clientIsConnected = true;

        internal ClientSession(TcpClient client)
        {
            Client = client;
        }

        internal void StartReceiving()
        {
            Task networkTask = new Task(async () =>
            {
                // Buffer for reading data
                Byte[] bytes = new Byte[2048];

                // Get a stream object for reading and writing
                NetworkStream unencryptedStream = Client.GetStream();
                SslStream sslStream = new SslStream(unencryptedStream);

                DecisionEngine engine = new DecisionEngine(new ClientContext());

                bool _initializedTls = false;

                while (_clientIsConnected)
                {
                    Stream rawOrSslStream = (_useStartTls) ? sslStream : unencryptedStream;

                    try
                    {
                        if (_useStartTls && !_initializedTls)
                        {
                            await sslStream.AuthenticateAsServerAsync(new SslServerAuthenticationOptions
                            {
                                ServerCertificate = SingletonContainer.GetCertificate(),
                            });
                            _initializedTls = true;
                        }

                        rawOrSslStream.Read(bytes, 0, bytes.Length);
                        await HandleAsync(bytes, rawOrSslStream, engine);
                    }
                    catch (Exception e)
                    {
                        ILogger? logger = SingletonContainer.GetLogger();
                        if (logger != null)
                        {
                            logger.LogException(e);
                        }

                        break;
                    }
                }

                Client.Close();
            });

            networkTask.Start();
        }

        private async Task HandleAsync(byte[] bytes, Stream stream, DecisionEngine engine)
        {
            PacketParser parser = new PacketParser();
            LdapMessage message = parser.TryParsePacket(bytes);

            List<LdapMessage> replies = await engine.GenerateReply(message);
            foreach (LdapMessage outMsg in replies)
            {
                if (outMsg.ProtocolOp.GetType() == typeof(Gatekeeper.LdapServerLibrary.Models.Operations.Response.UnbindDummyResponse))
                {
                    _clientIsConnected = false;
                    break;
                }
                byte[] msg = parser.TryEncodePacket(outMsg);
                stream.Write(msg, 0, msg.Length);

                if (outMsg.ProtocolOp.GetType() == typeof(Gatekeeper.LdapServerLibrary.Models.Operations.Response.ExtendedOperationResponse))
                {
                    var response = ((Gatekeeper.LdapServerLibrary.Models.Operations.Response.ExtendedOperationResponse)outMsg.ProtocolOp);
                    if (response.ResponseName == ExtendedRequestHandler.StartTLS)
                    {
                        _useStartTls = true;
                    }
                }
            }
        }
    }
}
