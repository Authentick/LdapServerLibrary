using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Security;
using System.Net.Sockets;
using System.Threading.Tasks;
using Gatekeeper.LdapServerLibrary.Engine;
using Gatekeeper.LdapServerLibrary.Models;
using Gatekeeper.LdapServerLibrary.Engine.Handler;
using System.Net;

namespace Gatekeeper.LdapServerLibrary.Network
{
    internal class ClientSession
    {
        internal readonly TcpClient Client;
        private bool _useStartTls;
        private bool _clientIsConnected = true;

        private const int ASN_LENGTH_INDICATOR = 1;
        private const int ASN_MAX_SINGLE_BYTE_LENGTH = 127;
        private const int ASN_LENGTH_PREFIX_COUNT = 2;

        internal ClientSession(TcpClient client)
        {
            Client = client;
        }

        private int GetMultiByteLength(byte lengthIndicator)
        {
            return (lengthIndicator >> 0) & 127;
        }

        public async Task<byte[]> ReadFullyAsync(Stream stream)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                List<Byte> PacketLength = new List<Byte>();
                Byte[] LengthBuffer = new byte[10];
                int streamPosition = 0;
                int? packetSize = null;
                bool isMultiByteSize = false;
                int? multiByteSize = null;

                while (true)
                {
                    byte[] buffer = new byte[1];
                    int read = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (streamPosition == ASN_LENGTH_INDICATOR)
                    {
                        int number = Convert.ToInt32(buffer[0]);
                        if (number <= ASN_MAX_SINGLE_BYTE_LENGTH)
                        {
                            packetSize = number + ASN_LENGTH_PREFIX_COUNT;
                        }
                        else
                        {
                            isMultiByteSize = true;
                            multiByteSize = GetMultiByteLength(buffer[0]);
                        }
                    }
                    else
                    {
                        if (isMultiByteSize && (streamPosition - ASN_LENGTH_PREFIX_COUNT) < multiByteSize)
                        {
                            PacketLength.Add(buffer[0]);
                        }
                        else if (isMultiByteSize && (streamPosition - ASN_LENGTH_PREFIX_COUNT) == multiByteSize)
                        {
                            string hexValue = BitConverter.ToString(PacketLength.ToArray()).Replace("-", "");
                            packetSize = Convert.ToInt32(hexValue, 16) + ASN_LENGTH_PREFIX_COUNT + PacketLength.Count;
                        }
                    }

                    ms.Write(buffer, 0, read);
                    streamPosition++;

                    if (read <= 0 || streamPosition == packetSize)
                    {
                        return ms.ToArray();
                    }
                }
            }
        }

        internal void StartReceiving()
        {
            Task networkTask = new Task(async () =>
            {
                NetworkStream unencryptedStream = Client.GetStream();
                SslStream sslStream = new SslStream(unencryptedStream);

                IPEndPoint? endpoint = (IPEndPoint?)Client.Client.RemoteEndPoint;
                if (endpoint == null)
                {
                    throw new Exception("IP address is null");
                }

                ClientContext clientContext = new ClientContext(endpoint.Address);
                DecisionEngine engine = new DecisionEngine(clientContext);

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

                        Byte[] data = await ReadFullyAsync(rawOrSslStream);

                        await HandleAsync(data, rawOrSslStream, engine);
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
            Gatekeeper.LdapPacketParserLibrary.Parser parser = new Gatekeeper.LdapPacketParserLibrary.Parser();
            LdapPacketParserLibrary.Models.LdapMessage message = parser.TryParsePacket(bytes);

            List<LdapMessage> replies = await engine.GenerateReply(message);
            foreach (LdapMessage outMsg in replies)
            {
                if (outMsg.ProtocolOp.GetType() == typeof(Gatekeeper.LdapServerLibrary.Models.Operations.Response.UnbindDummyResponse))
                {
                    _clientIsConnected = false;
                    break;
                }
                byte[] msg = (new Parser.PacketParser()).TryEncodePacket(outMsg);
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
