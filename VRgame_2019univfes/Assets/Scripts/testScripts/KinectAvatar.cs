using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
//using Windows.Kinect;

/*
*************************************
このクラスは神クラスです。
このコードはUDP受信機能も押し込んである。
*************************************
*/

public class KinectAvatar : MonoBehaviour {

    //ネットワーク関連
    int LOCAL_PORT = 2001;
    static UdpClient udp;
    Thread thread;
    static string rawtext;

    //キャリブレーションするときにつかう
    private Vector3 calibrationPos;
    private Vector3 rawPos;
    private Vector3 fixPos;
    private Transform floorPos;
    private float floorDistance;

    //自分の関節とUnityちゃんのボーンを入れるよう
    [SerializeField] GameObject Ref;
    [SerializeField] GameObject LeftUpLeg;
    [SerializeField] GameObject LeftLeg;
    [SerializeField] GameObject RightUpLeg;
    [SerializeField] GameObject RightLeg;
    [SerializeField] GameObject Spine1;
    [SerializeField] GameObject LeftArm;
    [SerializeField] GameObject LeftForeArm;
    [SerializeField] GameObject LeftHand;
    [SerializeField] GameObject RightArm;
    [SerializeField] GameObject RightForeArm;
    [SerializeField] GameObject RightHand;

    void Start () {
        udp = new UdpClient(AddressFamily.InterNetwork);
        IPEndPoint localEP = new IPEndPoint(IPAddress.Any, LOCAL_PORT);
        udp.Client.Bind(localEP);
        udp.Client.ReceiveTimeout = 0;
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start();

        //座標のキャリブレーションに使う
        floorPos = GameObject.Find("Cube").transform;
        //Debug.Log("position:" + floorPos.position.ToString("f7"));
        //Debug.Log("localScale:" + floorPos.localScale);
        floorDistance = floorPos.localScale.y / 2 + floorPos.position.y;
    }

    void Update () {

        Quaternion q;
        Quaternion comp2;

        string[] splitText = rawtext.Split('_');
        
        // 関節の回転を取得する
        if (splitText.Length > 0)
        {

            // 関節の回転を計算する 
            q = transform.rotation;
            transform.rotation = Quaternion.identity;

            comp2 = Quaternion.AngleAxis(90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));

            //ここで飛んできた値を気合でパース
            Quaternion[] receiveQuaternion = new Quaternion[splitText.Length];
            for(int i=0;i<splitText.Length;i++){
                string[] quaternionStr = splitText[i].Split(',');
                for(int j=0;j<quaternionStr.Length;j++){
                    receiveQuaternion[i][j] = float.Parse(quaternionStr[j]);
                }
            }

            Spine1.transform.rotation = receiveQuaternion[0];
            RightArm.transform.rotation = receiveQuaternion[1];
            RightForeArm.transform.rotation = receiveQuaternion[2];
            RightHand.transform.rotation = receiveQuaternion[3];
            LeftArm.transform.rotation = receiveQuaternion[4];
            LeftForeArm.transform.rotation = receiveQuaternion[5];
            LeftHand.transform.rotation = receiveQuaternion[6];
            RightUpLeg.transform.rotation = receiveQuaternion[7];
            RightLeg.transform.rotation = receiveQuaternion[8];
            LeftUpLeg.transform.rotation = receiveQuaternion[9];
            LeftLeg.transform.rotation = receiveQuaternion[10];

            // モデルの回転を設定する
            transform.rotation = q;

            // モデルの位置を移動する
            rawPos = new Vector3(receiveQuaternion[11][0],receiveQuaternion[11][1],-receiveQuaternion[11][2]);

            //kinectの初期値をとっとく
            //補正値初期化と補正値設定をまとめてやる
            if(Input.GetMouseButtonDown(0)){
                calibrationPos = new Vector3(0.0f,0.0f,0.0f);
                calibrationPos = rawPos;
                calibrationPos = new Vector3(calibrationPos.x,calibrationPos.y - floorDistance, calibrationPos.z);
            }
            //補正後の値 = 生の値 - 補正値
            fixPos = rawPos - calibrationPos;
            transform.position = fixPos;

        }
    }

    //終了したときにスレッドを止める
    void OnApplicationQuit()
    {
        thread.Abort();
    }

    //値を受信するところ、別スレッドで動かしてる。
    //※要メソッド名変更
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