using UnityEngine;
using System;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPReceiver : MonoBehaviour
{
    [SerializeField] private int LOCAL_PORT = 2001;
    static UdpClient udp;
    Thread thread;
    static string rawText;

    void Start ()
    {
        udp = new UdpClient(AddressFamily.InterNetwork);
        IPEndPoint localEP = new IPEndPoint(IPAddress.Any, LOCAL_PORT);
        udp.Client.Bind(localEP);
        udp.Client.ReceiveTimeout = 0;
        thread = new Thread(new ThreadStart(receiveValueThread));
        thread.Start(); 
    }

    void OnApplicationQuit()
    {
        thread.Abort();
    }

    private static void receiveValueThread()
    {
        while(true)
        {
            IPEndPoint remoteEP = null;
            byte[] data = udp.Receive(ref remoteEP);
            rawText = Encoding.ASCII.GetString(data);
        }
    }

    public string GetrawText(){
        
        string receiveRawText;
        receiveRawText = rawText;
        rawText = "";
        return receiveRawText;
        
        //return rawText;
    }
}

/*
参考ページ
https://qiita.com/nenjiru/items/8fa8dfb27f55c0205651
*/