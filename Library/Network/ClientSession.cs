using System;
using System.Net.Sockets;
using System.Threading.Tasks;
using LdapServer.Models;
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
            Task taskA = new Task(() =>
            {
                // Buffer for reading data
                Byte[] bytes = new Byte[2048];

                // Get a stream object for reading and writing
                NetworkStream stream = Client.GetStream();

                int i;
                string? data;

                // Loop to receive all the data sent by the client.
                while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                {
                    Byte[] cropped = new Byte[i];
                    Array.Copy(bytes, 0, cropped, 0, i);

                    PacketParser parser = new PacketParser();
                                        System.Console.WriteLine("asdf");
                    var foo = parser.TryParsePacket(cropped);
                    System.Console.WriteLine(foo);

                    // Translate data bytes to a ASCII string.
                    data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                    Console.WriteLine("Received: {0}", data);


                    LdapResult ldapResult = new LdapResult(LdapResult.ResultCodeEnum.InappropriateAuthentication, null, null);
                    BindResponse bindResponse = new BindResponse(ldapResult);
                    LdapMessage outMessage = new LdapMessage(1, bindResponse);

                    MessageEncoder encoder = new MessageEncoder();
                    byte[] msg = encoder.TryEncode(outMessage);




                    // Send back a response.
                    // FIXME
                    if (data.Contains("Manager"))
                    {
                        stream.Write(msg, 0, msg.Length);
                        Console.WriteLine("Foo" + msg);
                    }
                    else
                    {
                        //  client.Close();
                        // break;
                    }

                }
            });

            // Start the task.
            taskA.Start();

        }
    }
}