/*
* The MIT License (MIT)
* 
* Copyright (c) 2012-2014 Fredrik Holmstrom (fredrik.johan.holmstrom@gmail.com)
* 
* Permission is hereby granted, free of charge, to any person obtaining a copy
* of this software and associated documentation files (the "Software"), to deal
* in the Software without restriction, including without limitation the rights
* to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
* copies of the Software, and to permit persons to whom the Software is
* furnished to do so, subject to the following conditions:
* 
* The above copyright notice and this permission notice shall be included in
* all copies or substantial portions of the Software.
* 
* THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
* IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
* FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
* AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
* LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
* OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
* THE SOFTWARE.
*/

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace UdpKit.Examples.Chat {
    class ChatSerializer : UdpSerializer {
        public override bool Pack (UdpStream stream, ref object o) {
            // cast to string and get bytes
            string msg = (string) o;
            byte[] bytes = Encoding.UTF8.GetBytes(msg);

            // write length and bytes into buffer
            stream.WriteInt(bytes.Length);
            stream.WriteByteArray(bytes);

            return true;
        }

        public override bool Unpack (UdpStream stream, ref object o) {
            // read length and create array, then read bytes into array
            byte[] bytes = new byte[stream.ReadInt()];
            stream.ReadByteArray(bytes);

            // convert bytes to string
            o = Encoding.UTF8.GetString(bytes);
            return true;
        }
    }

    class Server {
        UdpSocket socket;
        List<UdpConnection> clients;

        public Server () {
            socket = UdpSocket.Create<UdpPlatformManaged, ChatSerializer>();
            socket.Start(new UdpEndPoint(UdpIPv4Address.Localhost, 14000));
            clients = new List<UdpConnection>();
        }

        public void Loop () {
            int connections = 0;
            UdpEvent ev;

            while (true) {
                while (socket.Poll(out ev)) {
                    switch (ev.EventType) {
                        case UdpEventType.Connected:
                            ++connections;

                            // log in local console
                            UdpLog.User("Client connected from {0}, total clients {1}", ev.Connection.RemoteEndPoint, connections);

                            // send welcome message
                            ev.Connection.Send("Welcome to the chat server!");

                            // send message to all other clients
                            SendToAllClients("Client connected from {0}", ev.Connection.RemoteEndPoint);

                            // add to client list
                            clients.Add(ev.Connection);
                            break;

                        case UdpEventType.Disconnected:
                            --connections;

                            // log in local console
                            UdpLog.User("Client at {0} disconnected, total clients {1}", ev.Connection.RemoteEndPoint, connections);

                            // remove from client list
                            clients.Remove(ev.Connection);

                            // Send message to all others
                            SendToAllClients("Client at {0} disconnected", ev.Connection.RemoteEndPoint);
                            break;

                        // When we receive, just forward to all clients
                        case UdpEventType.ObjectReceived:
                            SendToAllClients(ev.Object as string);
                            break;

                        // If lost, resend to connection it was lost on
                        case UdpEventType.ObjectLost:
                            ev.Connection.Send(ev.Object);
                            break;
                    }
                }

                // Simulate ~60fps game loop
                Thread.Sleep(16);
            }
        }

        void SendToAllClients (string message, params object[] args) {
            message = string.Format(message, args);

            foreach (UdpConnection connection in clients) {
                connection.Send(message);
            }
        }
    }

    class Client {

        delegate string ReadLine ();

        UdpSocket socket;

        public Client () {
            socket = UdpSocket.Create<UdpPlatformManaged, ChatSerializer>();
            socket.Start(UdpEndPoint.Any);
            socket.Connect(new UdpEndPoint(UdpIPv4Address.Localhost, 14000));
        }

        public void Loop () {
            StreamReader input = new StreamReader(Console.OpenStandardInput());
            Char[] buffer = new Char[1024];
            ReadLine read = Console.ReadLine;
            IAsyncResult result = null;
            UdpConnection connection = null;
            
            UdpEvent ev;

            while (true) {
                while (socket.Poll(out ev)) {
                    switch (ev.EventType) {
                        case UdpEventType.Connected:
                            UdpLog.User("Connected to server at {0}", ev.Connection.RemoteEndPoint);
                            connection = ev.Connection;
                            break;

                        case UdpEventType.Disconnected:
                            UdpLog.User("Disconnected from server at {0}", ev.Connection.RemoteEndPoint);
                            connection = null;
                            break;

                        case UdpEventType.ObjectReceived:
                            Console.WriteLine(": " + (ev.Object as string));
                            break;
                    }
                }

                if (result == null) {
                    result = read.BeginInvoke(null, null);
                }

                if (result.IsCompleted) {
                    if (connection != null) {
                        connection.Send(read.EndInvoke(result));
                    }

                    result = read.BeginInvoke(null, null);
                }

                // Simulate ~60fps game loop
                Thread.Sleep(16);
            }
        }

    }

    class Program {
        static void Main (string[] args) {
            Console.WriteLine("Example: Chat");
            Console.WriteLine("Press [S] to start server");
            Console.WriteLine("Press [C] to start client");
            Console.Write("... ");

            UdpLog.SetWriter((l, m) => Console.WriteLine(m));

            switch (Console.ReadKey(true).Key) {
                case ConsoleKey.S:
                    Console.WriteLine("Server");
                    new Server().Loop();
                    break;

                case ConsoleKey.C:
                    Console.WriteLine("Client");
                    new Client().Loop();
                    break;

                default:
                    Main(args);
                    break;
            }
        }
    }
}
