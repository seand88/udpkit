using System;

namespace UdpKit {
    public enum UdpReliableRecvResult {
        Old,
        OutOfBounds,
        AlreadyExists,
        Added
    }

    public class UdpReliableBuffer<T> where T : IUdpSequencedObject {
        UdpReliableRecvQueue<T> recv;
        UdpReliableSendQueue<T> send;

        public int SequenceBits {
            get;
            private set;
        }

        public UdpReliableBuffer (uint windowSize) {
            if (windowSize < (1 << 2)) throw new ArgumentException("Must be >= (1 << 2)", "windowSize");
            if (windowSize > (1 << 14)) throw new ArgumentException("Must be <= (1 << 14)", "windowSize");
            if (UdpMath.IsPowerOfTwo(windowSize) == false) throw new ArgumentException("Must be a power of two", "windowSize");

            SequenceBits = UdpMath.HighBit(windowSize) + 2;

            recv = new UdpReliableRecvQueue<T>(SequenceBits);
            send = new UdpReliableSendQueue<T>(SequenceBits);
        }

        public bool Send_TryEnqueue (T value) {
            return send.TryQueueForSending(value);
        }

        public bool Send_TryGetForPacking (out T value) {
            return send.TryGetForPacking(out value);
        }

        public bool Send_TryGetForRemoval (out T value) {
            return send.TryGetForPacking(out value);
        }

        public bool Recv_TryEnqueue (T value, out UdpReliableRecvResult result) {
            return recv.TryEnqueueForDelivery(value, out result);
        }

        public bool Recv_TryDeliver (out T value) {
            return recv.TryGetForDelivery(out value);
        }
    }
}
