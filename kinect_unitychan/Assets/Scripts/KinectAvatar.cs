using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Windows.Kinect;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

/*********************************
これは送信側のクラス
**********************************/

public class KinectAvatar : MonoBehaviour {

    //UDP通信
    [SerializeField] private string ipaddress = "localhost";
    [SerializeField] private int usePort = 2001;
    public static string host = "localhost";
    public static int port = 2001;
    private static UdpClient client;
    Thread thread;
    static string rawtext;

    //タイマー処理？？？ 30fps
    private float loopTime = 1.0f / 30.0f;


    //キャリブレーションするときにつかう
    private Vector3 calibrationPos;
    private Vector3 posVector;
    private Transform floorPos;
    private float floorDistance;

    //カメラ関連
    //spineの子にしたらいい感じになった
    /*
    GameObject mainCamObj;
    Camera cam;
    */

    //カメラがくがく補正
    [SerializeField] private const int keepFrame = 60;
    //ここはposition
    Queue<Vector3> posQue = new Queue<Vector3>();
    private Vector3 lowPassBuffer,sendPos;
    //ここはspineのQuaternion
    Queue<Quaternion> quaternionQue = new Queue<Quaternion>();
    private Quaternion quaternionLowPassBuffer,sendQuaternion;
    
    [SerializeField] BodySourceManager bodySourceManager;

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

    Quaternion comp2;
    CameraSpacePoint pos;

    // Use this for initialization
    void Start () {
        calibrationPos = new Vector3(0.0f,0.0f,0.0f);
        comp2 = Quaternion.AngleAxis(90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
        //IPアドレスとポートの設定
        host = ipaddress;
        port = usePort;
        client = new UdpClient();
        //ここはスレッドを起動するところ
        /*********************************
        thread = new Thread(new ThreadStart(ThreadMethod));
        thread.Start(); 
        **********************************/

        //座標のキャリブレーションに使う
        floorPos = GameObject.Find("Cube").transform;
        //Debug.Log("position:" + floorPos.position.ToString("f7"));
        //Debug.Log("localScale:" + floorPos.localScale);
        floorDistance = floorPos.localScale.y / 2 + floorPos.position.y;
        
        //ここでコルーチンをスタートさせる
        StartCoroutine(SendByte());

        //カメラ関連
        /*
        mainCamObj = GameObject.FindGameObjectWithTag("MainCamera");
        cam = mainCamObj.GetComponent<Camera>();
        */

        //カメラ　ローパスフィルター処理
        lowPassBuffer = new Vector3(0.0f,0.0f,0.0f);
        quaternionLowPassBuffer = new Quaternion(0.0f,0.0f,0.0f,0.0f);
        sendPos = new Vector3(0.0f,0.0f,0.0f);
        sendQuaternion = new Quaternion(0.0f,0.0f,0.0f,0.0f);
        for(int i=0;i<keepFrame;i++){
            posQue.Enqueue(new Vector3(0.0f,0.0f,0.0f));
            quaternionQue.Enqueue(new Quaternion(0.0f,0.0f,0.0f,0.0f));
        }
    }

    // Update is called once per frame
    void Update () {
        //最初に追尾している人のBodyデータを取得する
        Body body = bodySourceManager.GetData().FirstOrDefault(b => b.IsTracked);
        
        // Kinectを斜めに置いてもまっすぐにするようにする
        var floorPlane = bodySourceManager.FloorClipPlane;
        Quaternion comp = Quaternion.FromToRotation(
            new Vector3(-floorPlane.X, floorPlane.Y, floorPlane.Z), Vector3.up);

        Quaternion SpineBase;
        Quaternion SpineMid;
        Quaternion SpineShoulder;
        Quaternion ShoulderLeft;
        Quaternion ShoulderRight;
        Quaternion ElbowLeft;
        Quaternion WristLeft;
        Quaternion HandLeft;
        Quaternion ElbowRight;
        Quaternion WristRight;
        Quaternion HandRight;
        Quaternion KneeLeft;
        Quaternion AnkleLeft;
        Quaternion KneeRight;
        Quaternion AnkleRight;

        Quaternion q;
        //Quaternion comp2;
        //CameraSpacePoint pos;

        // 関節の回転を取得する
        if (body != null)
        {
            var joints = body.JointOrientations;

            //Kinectの関節回転情報をUnityのクォータニオンに変換
            SpineBase = joints[JointType.SpineBase].Orientation.ToQuaternion(comp);
            SpineMid = joints[JointType.SpineMid].Orientation.ToQuaternion(comp);
            SpineShoulder = joints[JointType.SpineShoulder].Orientation.ToQuaternion(comp);
            ShoulderLeft = joints[JointType.ShoulderLeft].Orientation.ToQuaternion(comp);
            ShoulderRight = joints[JointType.ShoulderRight].Orientation.ToQuaternion(comp);
            ElbowLeft = joints[JointType.ElbowLeft].Orientation.ToQuaternion(comp);
            WristLeft = joints[JointType.WristLeft].Orientation.ToQuaternion(comp);
            HandLeft = joints[JointType.HandLeft].Orientation.ToQuaternion(comp);
            ElbowRight = joints[JointType.ElbowRight].Orientation.ToQuaternion(comp);
            WristRight = joints[JointType.WristRight].Orientation.ToQuaternion(comp);
            HandRight = joints[JointType.HandRight].Orientation.ToQuaternion(comp);
            KneeLeft = joints[JointType.KneeLeft].Orientation.ToQuaternion(comp);
            AnkleLeft = joints[JointType.AnkleLeft].Orientation.ToQuaternion(comp);
            KneeRight = joints[JointType.KneeRight].Orientation.ToQuaternion(comp);
            AnkleRight = joints[JointType.AnkleRight].Orientation.ToQuaternion(comp);

            // 関節の回転を計算する 

            //----------------ここまでを処理して送る------------------
            //----------------以下で代入-----------------------------
            q = transform.rotation;
            transform.rotation = Quaternion.identity;

            //comp2 = Quaternion.AngleAxis(90, new Vector3(0, 1, 0)) * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
                             

            Spine1.transform.rotation = SpineMid * comp2;
            //ローパスフィルター処理
            Quaternion dequeueQuaternion;
            dequeueQuaternion = quaternionQue.Dequeue();
            for(int i=0;i<4;i++){
                quaternionLowPassBuffer[i] -= dequeueQuaternion[i];
                quaternionLowPassBuffer[i] += Spine1.transform.rotation[i];
                sendQuaternion[i] = quaternionLowPassBuffer[i] / (float)keepFrame;
            }
            quaternionQue.Enqueue(Spine1.transform.rotation);
            Spine1.transform.rotation = sendQuaternion;
            

            RightArm.transform.rotation = ElbowRight * comp2;
            RightForeArm.transform.rotation = WristRight * comp2;
            RightHand.transform.rotation = HandRight * comp2;

            LeftArm.transform.rotation = ElbowLeft * comp2;
            LeftForeArm.transform.rotation = WristLeft * comp2;
            LeftHand.transform.rotation = HandLeft * comp2;

            RightUpLeg.transform.rotation = KneeRight * Quaternion.AngleAxis(90, new Vector3(0, 0, 1)) * Quaternion.AngleAxis(180, new Vector3(0, 1, 0));
            RightLeg.transform.rotation = AnkleRight * Quaternion.AngleAxis(90, new Vector3(0, 0, 1)) * Quaternion.AngleAxis(180, new Vector3(0, 1, 0));

            LeftUpLeg.transform.rotation = KneeLeft * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));
            LeftLeg.transform.rotation = AnkleLeft * Quaternion.AngleAxis(-90, new Vector3(0, 0, 1));

            // モデルの回転を設定する
            transform.rotation = q;

            // モデルの位置を移動する
            pos = body.Joints[JointType.SpineMid].Position;
            posVector = new Vector3(pos.X, pos.Y, -pos.Z);
            //ローパスフィルター処理
            lowPassBuffer -= posQue.Dequeue();
            lowPassBuffer += posVector;
            posQue.Enqueue(posVector);
            sendPos = lowPassBuffer / (float)keepFrame;
            //Debug.Log("noLowPass:" + posVector.ToString("f7"));
            Debug.Log("LowPass:" + sendPos.ToString("f7"));

            //ここは送信側で様子を見るための処理
            //kinectの初期値をとっとく
            if(Input.GetMouseButtonDown(0)){
                calibrationPos = sendPos;
                //calibrationPos = posVector;
                calibrationPos = new Vector3(calibrationPos.x,calibrationPos.y - floorDistance, calibrationPos.z);
            }
            if(Input.GetMouseButtonDown(1)){
                calibrationPos = new Vector3(0.0f,0.0f,0.0f);
            }

            /********床との距離を取りなさいby東郷***************/
            //unitychanの中心座標？は足
            //床との計算はscale.Y/2 + position.X;

            //posとcalibrationPosを引く
            sendPos = sendPos - calibrationPos;
            //posVector = posVector - calibrationPos;
            //Ref.transform.position = new Vector3(pos.X, pos.Y + this.transform.position.y, -pos.Z);
            //transform.position = new Vector3(pos.X, pos.Y, -pos.Z);
            transform.position = sendPos;
            //transform.position = posVector;
            //Debug.Log("this:" + this.transform.position.ToString("f7"));
            //Debug.Log("Ref:" + Ref.transform.position.ToString("f7"));

            //ここで頭の座標をとってくる！！！
            //TrackCameraPos(body);
        }
    }

    //コルーチンをつかうのはどうだろうか
    //ここはC#のthreadを使ってるところ
    /******************************
    private static void ThreadMethod()
    {
        while(true)
        {
            byte[] dgram = Encoding.UTF8.GetBytes("hello!");
            client.Send(dgram, dgram.Length, host, port);
        }
    }
    void OnApplicationQuit()
    {
        client.Close();
        thread.Abort();
    }
    **********************************/

    //ここはコルーチンで実装したところ
    IEnumerator SendByte() {
        while(true){
            //byte[] dgram = Encoding.UTF8.GetBytes("hello!" + DateTime.Now.ToString());
            /*
            byte[] dgram = Encoding.UTF8.GetBytes(
                Spine1.transform.rotation.ToString("f7") + " " +
                RightArm.transform.rotation.ToString("f7") + "_" +
                RightForeArm.transform.rotation.ToString("f7") + "_" +
                RightHand.transform.rotation.ToString("f7") + "_" +
                LeftArm.transform.rotation.ToString("f7") + "_" +
                LeftForeArm.transform.rotation.ToString("f7") + "_" +
                LeftHand.transform.rotation.ToString("f7") + "_" +
                RightUpLeg.transform.rotation.ToString("f7") + "_" +
                RightLeg.transform.rotation.ToString("f7") + "_" +
                LeftUpLeg.transform.rotation.ToString("f7") + "_" +
                LeftLeg.transform.rotation.ToString("f7")
            );
            */
            byte[] dgram = Encoding.UTF8.GetBytes(MakeSendStr());
            client.Send(dgram, dgram.Length, host, port);
            yield return new WaitForSeconds(loopTime);
        }
    }

    private string MakeSendStr(){
        string str;
        str =
            RemoveParentheses(Spine1.transform.rotation.ToString("f7")) + "_" +
            //RemoveParentheses(sendQuaternion.ToString("f7")) + "_" +
            RemoveParentheses(RightArm.transform.rotation.ToString("f7")) + "_" +
            RemoveParentheses(RightForeArm.transform.rotation.ToString("f7")) + "_" +
            RemoveParentheses(RightHand.transform.rotation.ToString("f7")) + "_" +
            RemoveParentheses(LeftArm.transform.rotation.ToString("f7")) + "_" +
            RemoveParentheses(LeftForeArm.transform.rotation.ToString("f7")) + "_" +
            RemoveParentheses(LeftHand.transform.rotation.ToString("f7")) + "_" +
            RemoveParentheses(RightUpLeg.transform.rotation.ToString("f7")) + "_" +
            RemoveParentheses(RightLeg.transform.rotation.ToString("f7")) + "_" +
            RemoveParentheses(LeftUpLeg.transform.rotation.ToString("f7")) + "_" +
            RemoveParentheses(LeftLeg.transform.rotation.ToString("f7")) + "_" +
            RemoveParentheses(posVector.ToString("f7"));
            //RemoveParentheses(sendPos.ToString("f7"));
            //pos.X + "," + pos.Y + "," + pos.Z;
        return str;
    }



    private string RemoveParentheses(string s){
        return s.Substring(1,s.Length-2);
    }

    //これは自作した関数　カメラの座標を変える
    /*
    private void TrackCameraPos(Body body){
        Vector3 tmp = GetVector3FromJoint(body.Joints[JointType.Head]);
        Debug.Log("name:"+JointType.Head + " x:"+ tmp[0] + " y:"+ tmp[1] + " z:"+ tmp[2]);
        //tmp[0] = -tmp[0];
        cam.transform.position = tmp;
    }

    private static Vector3 GetVector3FromJoint(Windows.Kinect.Joint joint)
    {
        return new Vector3(joint.Position.X * 10, joint.Position.Y * 10, joint.Position.Z * 10);
    }
    */
}