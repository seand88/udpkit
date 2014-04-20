using System.Collections.Generic;

namespace UdpKit {
  public class UdpStreamPool {
    readonly int size = 0;
    readonly Stack<UdpStream> pool = new Stack<UdpStream>();

    public int Size {
      get { return size; }
    }

    internal UdpStreamPool (UdpSocket s) {
      size = s.Config.MtuMax * 2;
    }

    public void Release (UdpStream stream) {
      UdpAssert.Assert(stream.pooled == false);

      lock (pool) {
        pool.Push(stream);
        stream.pooled = true;
      }
    }

    public UdpStream Acquire () {
      UdpStream stream = null;

      lock (pool) {
        if (pool.Count > 0) {
          stream = pool.Pop();
        }
      }

      if (stream == null) {
        stream = new UdpStream(new byte[size]);
      }

      UdpAssert.Assert(stream.pooled);
      stream.pooled = false;
      return stream;
    }

    public void Free () {
      lock (pool) {
        while (pool.Count > 0) {
          pool.Pop();
        }
      }
    }
  }
}