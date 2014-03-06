using System;

namespace UdpKit {
    class UdpReliableRecvQueue<T> where T : IUdpSequencedObject {
        struct Node {
            public bool Received;
            public T Value;
        }

        int tail;
        int mask;
        int sequenceShift;
        uint sequenceNext;
        uint sequenceMask;
        readonly Node[] nodes;

        public UdpReliableRecvQueue (int sequenceBits) {
            if (sequenceBits < 4 && sequenceBits > 32) {
                throw new ArgumentException("sequenceBits must be >= 4 and <= 32");
            }

            nodes = new Node[1 << (sequenceBits - 2)];
            mask = nodes.Length - 1;

            sequenceShift = 32 - sequenceBits;
            sequenceMask = (1u << sequenceBits) - 1u;
            sequenceNext = 0;
        }

        public bool TryGetForDelivery (out T value) {
            Node n = nodes[tail];

            if (n.Received) {
                value = n.Value;
                nodes[tail] = default(Node);

                tail += 1;
                tail &= mask;

                sequenceNext = value.Sequence + 1u;
                sequenceNext &= sequenceMask;
            } else {
                value = default(T);
            }

            return n.Received;
        }

        public bool TryEnqueueForDelivery (T value, out UdpReliableRecvResult result) {
            int distance = SequenceDistance(value.Sequence, sequenceNext, sequenceShift);
            int index = (tail + distance) & mask;

            if (distance <= -nodes.Length || distance >= nodes.Length) {
                result = UdpReliableRecvResult.OutOfBounds;
                return false;
            }

            if (distance < 0) {
                result = UdpReliableRecvResult.Old;
                return false;
            }

            if (nodes[index].Received) {
                result = UdpReliableRecvResult.AlreadyExists;
                return false;
            }

            nodes[index].Received = true;
            nodes[index].Value = value;

            result = UdpReliableRecvResult.Added;
            return true;
        }

        static int SequenceDistance (uint from, uint to, int shift) {
            from <<= shift;
            to <<= shift;
            return ((int) (from - to)) >> shift;
        }
    }
}
