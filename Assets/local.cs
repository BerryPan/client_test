using UnityEngine;
using System.Collections;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.IO;
using System.Linq;
using Google.Protobuf;


public class local : MonoBehaviour
{
    void Start()
    {
        ConncetServer_local();
    }


    void ConncetServer_local()
    {
        IPAddress ipAdr = IPAddress.Parse("127.0.0.1");
        IPEndPoint ipEp = new IPEndPoint(ipAdr, 1234);
        Socket clientScoket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        clientScoket.Connect(ipEp);
        for (int i = 0; i < 100; ++i)
        {
            float xx = GameObject.Find("def").GetComponent<Transform>().position.x;
            float yy = GameObject.Find("def").GetComponent<Transform>().position.y;
            float zz = GameObject.Find("def").GetComponent<Transform>().position.z;
            float rx = GameObject.Find("abc").GetComponent<Transform>().rotation.x;
            float ry = GameObject.Find("abc").GetComponent<Transform>().rotation.y;
            float rz = GameObject.Find("abc").GetComponent<Transform>().rotation.z;
            Local loc = new Local{ PosX = xx, PosY = yy, PosZ = zz , Name = "def"};
            byte[] concent = loc.ToByteArray();
            clientScoket.Send(concent);
            byte[] response = new byte[1024];
            int len_recv = clientScoket.Receive(response);
            byte[] data = response.Take(len_recv).ToArray();
            Another another = Another.Parser.ParseFrom(data);
            print(another);
            len_recv = clientScoket.Receive(response);
        }


        clientScoket.Shutdown(SocketShutdown.Both);
        clientScoket.Close();
    }
}