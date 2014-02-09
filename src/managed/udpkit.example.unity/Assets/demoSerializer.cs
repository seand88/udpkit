using UdpKit;

public class demoSerializer : UdpSerializer {
    public override bool Pack (UdpStream stream, ref object o) {
        throw new System.NotImplementedException();
    }

    public override bool Unpack (UdpStream stream, ref object o) {
        throw new System.NotImplementedException();
    }
}
