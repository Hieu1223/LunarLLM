using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Net;
using UnityEngine;
using System.Text;

public class UDPSend : MonoBehaviour
{
    Socket server;
    EndPoint Remote;
    private void Start()
    {
        IPEndPoint ipep = new IPEndPoint(
                      IPAddress.Parse("127.0.0.1"), 8080);

        server = new Socket(AddressFamily.InterNetwork,
                       SocketType.Dgram, ProtocolType.Udp);

        byte[] data = Encoding.Default.GetBytes("Hello");
        server.SendTo(data, data.Length, SocketFlags.None, ipep);

        IPEndPoint sender = new IPEndPoint(IPAddress.Any, 0);
        Remote = (EndPoint)ipep;
    }
    public void Send(string str)
    {
        server.SendTo(Encoding.ASCII.GetBytes(str), Remote);
    }
    private void OnDestroy()
    {
        server.Close();
    }
}
