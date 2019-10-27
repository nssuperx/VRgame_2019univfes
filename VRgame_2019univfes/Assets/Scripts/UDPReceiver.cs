using UnityEngine;
using System.Collections;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class UDPReceiver : MonoBehaviour
{
    int LOCAL_PORT = 2001;
    static UdpClient udp;
    Thread thread;
    static string rawtext;
    GameObject mainCamObj;
    Camera cam;

    void Start ()
    {
        mainCamObj = GameObject.FindGameObjectWithTag("MainCamera");
        cam = mainCamObj.GetComponent<Camera>();
        udp = new UdpClient(AddressFamily.InterNetwork);
        IPEndPoint localEP = new IPEndPoint(IPAddress.Any, LOCAL_PORT);
        udp.Client.Bind(localEP);
        udp.Client.ReceiveTimeout = 0;
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start(); 
    }

    void Update ()
    {
        string[] splitText = rawtext.Split(',');
        Vector3 headPos = new Vector3(
            float.Parse(splitText[0]),
            float.Parse(splitText[1]),
            float.Parse(splitText[2]));

        Debug.Log(headPos);
        cam.transform.position = headPos;
    }

    void OnApplicationQuit()
    {
        thread.Abort();
    }

    private static void ThreadMethod()
    {
        while(true)
        {
            IPEndPoint remoteEP = null;
            byte[] data = udp.Receive(ref remoteEP);
            rawtext = Encoding.ASCII.GetString(data);
            //Debug.Log(rawtext);
        }
    } 
}

/*
参考ページ
https://qiita.com/nenjiru/items/8fa8dfb27f55c0205651
*/