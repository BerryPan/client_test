using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Linq;
using Google.Protobuf;

public class client : MonoBehaviour
{
    void Start()
    {
        ConncetServer();
    }


    void ConncetServer()
    {
        IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
        IPEndPoint ipEp = new IPEndPoint(ipAdr, 1234);
        Socket clientScoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientScoket.Connect(ipEp);
        for(int i = 0; i < 100; ++i)
        {
            Local loc = new Local { PosX = 12, PosY = 12, PosZ = 32, Name = "abc", RotX = 12, RotY = 123, RotZ = 34};
            byte[] concent = loc.ToByteArray();
            clientScoket.Send(concent);

            byte[] response = new byte[1024];
            int len_recv = clientScoket.Receive(response);
            byte[] data = response.Take(len_recv).ToArray();
            Another another = Another.Parser.ParseFrom(data);
            GameObject.Find("abc").GetComponent<Transform>().position = new Vector3(another.PosX, another.PosY, another.PosZ);
            GameObject.Find("abc").GetComponent<Transform>().rotation = Quaternion.Euler(another.RotX, another.RotY, another.RotZ);
            len_recv = clientScoket.Receive(response);

        }
        

        clientScoket.Shutdown(SocketShutdown.Both);
        clientScoket.Close();
    }
}