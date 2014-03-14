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

//#define DISABLE_AUTO_ACCEPT
//#define ENABLE_MANUAL_ACCEPT

using System;
using System.Threading;

namespace UdpKit.Examples.Simple {
    class DummySerializer : UdpSerializer {
        public override bool Pack (UdpStream stream, ref object o) {
            throw new NotImplementedException();
        }

        public override bool Unpack (UdpStream stream, ref object o) {
            throw new NotImplementedException();
        }
    }

    class Program {
        static void Client () {
            UdpSocket client = UdpSocket.Create<UdpPlatformManaged, DummySerializer>();
            client.Start(UdpEndPoint.Any);
            client.Connect(new UdpEndPoint(UdpIPv4Address.Localhost, 14000));

            while (true) {
                UdpEvent ev;

                while (client.Poll(out ev)) {
                    UdpLog.User("Event raised {0}", ev.EventType);

                    switch (ev.EventType) {
                        case UdpEventType.Connected:
                            UdpLog.User("Connected to server at {0}", ev.Connection.RemoteEndPoint);
                            break;

#if DISABLE_AUTO_ACCEPT
                        case UdpEventType.ConnectFailed:
                            UdpLog.User("Connection to {0} failed", ev.EndPoint);
                            break;
#endif
                    }
                }

                // Simulate ~60fps game loop
                Thread.Sleep(16);
            }
        }

        static void Server () {
#if DISABLE_AUTO_ACCEPT
            UdpConfig config = new UdpConfig();
            config.AutoAcceptIncommingConnections = false;
#else
            UdpConfig config = new UdpConfig();
#endif
            UdpSocket server = UdpSocket.Create<UdpPlatformManaged, DummySerializer>(config);
            server.Start(new UdpEndPoint(UdpIPv4Address.Localhost, 14000));

            while (true) {
                UdpEvent ev;

                while (server.Poll(out ev)) {
                    UdpLog.User("Event raised {0}", ev.EventType);

                    switch (ev.EventType) {
                        case UdpEventType.Connected:
                            UdpLog.User("Client connected from {0}", ev.Connection.RemoteEndPoint);
                            break;

#if ENABLE_MANUAL_ACCEPT
                        case UdpEventType.ConnectRequest:
                            UdpLog.User("Connection requested from {0}", ev.EndPoint);
                            server.Accept(ev.EndPoint);
                            break;
#endif
                    }
                }

                // Simulate ~60fps game loop
                Thread.Sleep(16);
            }
        }

        static void Main (string[] args) {
            Console.WriteLine("Example: Simple");
            Console.WriteLine("Press [S] to start server");
            Console.WriteLine("Press [C] to start client");
            Console.Write("... ");

            UdpLog.SetWriter((l, m) => Console.WriteLine(m));

            switch (Console.ReadKey(true).Key) {
                case ConsoleKey.S:
                    Console.WriteLine("Server");
                    Server();
                    break;

                case ConsoleKey.C:
                    Console.WriteLine("Client");
                    Client();
                    break;

                default:
                    Main(args);
                    break;
            }
        }
    }
}
