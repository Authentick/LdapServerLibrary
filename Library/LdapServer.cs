using System;
using System.Net;
using System.Net.Sockets;
using LdapServer.Models;
using LdapServer.Models.Operations.Response;

namespace LdapServer
{
    public class LdapServer
    {
        public void Start()
        {
            TcpListener server = null;
            try
            {
                // Set the TcpListener on port 389.
                Int32 port = 389;
                IPAddress localAddr = IPAddress.Parse("127.0.0.1");

                // TcpListener server = new TcpListener(port);
                server = new TcpListener(localAddr, port);

                // Start listening for client requests.
                server.Start();

                // Buffer for reading data
                Byte[] bytes = new Byte[2048];
                String data = null;

                // Enter the listening loop.
                while (true)
                {
                    Console.Write("Waiting for a connection... ");

                    // Perform a blocking call to accept requests.
                    // You could also use server.AcceptSocket() here.
                    TcpClient client = server.AcceptTcpClient();
                    Console.WriteLine("Connected!");

                    data = null;

                    // Get a stream object for reading and writing
                    NetworkStream stream = client.GetStream();

                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        Byte[] cropped = new Byte[i];
                        Array.Copy(bytes, 0, cropped, 0, i);

                        Class1 class1 = new Class1();
                        class1.Foo(cropped);

                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                        Console.WriteLine("Received: {0}", data);


                        LdapResult ldapResult = new LdapResult(LdapResult.ResultCodeEnum.InappropriateAuthentication, null, null);
                        BindResponse bindResponse = new BindResponse(ldapResult);
                        LdapMessageOut outMessage = new LdapMessageOut(1, bindResponse);

                        MessageEncoder encoder = new MessageEncoder();
                        byte[] msg = encoder.TryEncode(outMessage);
                        



                        // Send back a response.
                        // FIXME
                        if (data.Contains("Manager"))
                        {
                            stream.Write(msg, 0, msg.Length);
                            Console.WriteLine("Foo" + msg);
                        } else {
                          //  client.Close();
                           // break;
                        }

                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                // Stop listening for new clients.
                server.Stop();
            }

            Console.WriteLine("\nHit enter to continue...");
            Console.Read();

            //  Class1 class1 = new Class1();
            // class1.Foo();

            Console.WriteLine("Hello World!");
        }
    }
}