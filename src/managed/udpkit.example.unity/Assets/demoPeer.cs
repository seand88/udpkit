using UdpKit;
using UnityEngine;

public class demoPeer : MonoBehaviour {
    UdpSocket socket;

    [HideInInspector]
    internal bool isServer;
    
    [HideInInspector]
    internal string serverAddress = "127.0.0.1:14000";

    void Awake () {
        UdpLog.SetWriter((s, lvl) => Debug.Log(s));
    }

    void Start () {
        socket = UdpKitUnityUtils.CreatePlatformSpecificSocket<demoSerializer>();

        if (isServer) {
            socket.Start(UdpEndPoint.Parse(serverAddress));
        } else {
            socket.Start(UdpEndPoint.Any);
			socket.Connect(UdpEndPoint.Parse(serverAddress));
        }
    }

    void OnDestroy () {
        socket.Close();
    }

    void Update () {
        UdpEvent ev;

        while (socket.Poll(out ev)) {
            switch (ev.EventType) {
                case UdpEventType.Connected:
                    UdpLog.User("Client connect from {0}", ev.Connection.RemoteEndPoint);
                    break;
            }
        }
    }
}
