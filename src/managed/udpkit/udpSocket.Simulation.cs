using System;
using System.Collections.Generic;

namespace UdpKit {
#if DEBUG
    partial class UdpSocket {
        struct DelayedPacket {
            public UdpEndPoint EndPoint;
            public byte[] Data;
            public int Length;
            public uint Time;
        }

        readonly Queue<byte[]> delayedBuffers = new Queue<byte[]>();
        readonly Queue<DelayedPacket> delayedPackets = new Queue<DelayedPacket>();

        bool ShouldDelayPacket {
            get { return Config.SimulatedPingMin > 0 && Config.SimulatedPingMax > 0 && Config.SimulatedPingMin < Config.SimulatedPingMax; }
        }

        bool ShouldDropPacket {
            get { return random.NextDouble() < Config.SimulatedLoss; }
        }

        partial void DelayPacket (UdpEndPoint ep, byte[] data, int length) {
            DelayedPacket packet = new DelayedPacket();
            packet.Data = delayedBuffers.Count > 0 ? delayedBuffers.Dequeue() : new byte[Config.MtuMax * 2];
            packet.EndPoint = ep;
            packet.Length = length;
            packet.Time = GetCurrentTime() + (uint) random.Next(Config.SimulatedPingMin, Config.SimulatedPingMax);

            // copy entire buffer into packets data buffer
            Array.Copy(data, 0, packet.Data, 0, data.Length);

            // put on delay queue
            delayedPackets.Enqueue(packet);
        }

        partial void RecvDelayedPackets () {
            while (delayedPackets.Count > 0 && GetCurrentTime() >= delayedPackets.Peek().Time) {
                DelayedPacket packet = delayedPackets.Dequeue();
                UdpStream stream = GetReadStream();

                // copy data into streams buffer
                Array.Copy(packet.Data, 0, stream.Data, 0, packet.Data.Length);

                // clear packet data
                Array.Clear(packet.Data, 0, packet.Data.Length);

                // receive packet
                RecvNetworkPacket(packet.EndPoint, stream, packet.Length);

                // put packet data buffer back in pool
                delayedBuffers.Enqueue(packet.Data);
            }
        }
    }
#else
    partial class UdpSocket {
        bool ShouldDelayPacket {
            get { return false; }
        }

        bool ShouldDropPacket {
            get { return false; }
        }
    }
#endif
}
