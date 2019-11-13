using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*********************************
このクラスはunityちゃんを動かします
**********************************/

/*********************************
やることリスト
0. 新しい値が来てるとき来てないときで処理をしっかり分ける。 <- いまここ
0.1 新しい値来てないときとれるstring(rawStr)の値確認
1. unityちゃん本体の回転初期化処理を止めてみる。
2. Quaternion線形補完を試す
**********************************/

public class KinectAvatar : MonoBehaviour {

    //ネットワーク関連
    private UDPReceiver udpReceiver;
    private const int receiveQuaternionNum = 11;

    //キャリブレーションするときにつかう
    private Vector3 calibrationPos;
    private Vector3 rawPos;
    private Vector3 fixPos;
    [SerializeField] private Transform floorPos;
    private float floorDistance;
    private float touchTime;

    //カメラ　ローパスフィルターもどき
    [SerializeField] private const int keepFrame = 10;
    //ここはposition
    Queue<Vector3> posQue = new Queue<Vector3>();
    private Vector3 lowPassBuffer,filteredPos;
    //ここはspineのQuaternion
    Queue<Quaternion> quaternionQue = new Queue<Quaternion>();
    private Quaternion quaternionLowPassBuffer,filteredQuaternion;

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
        udpReceiver = GetComponent<UDPReceiver>();

        //座標のキャリブレーションに使う
        touchTime = 0.0f;
        floorDistance = floorPos.localScale.y / 2 + floorPos.position.y;

        //カメラ　ローパスフィルター処理
        lowPassBuffer = new Vector3(0.0f,0.0f,0.0f);
        quaternionLowPassBuffer = new Quaternion(0.0f,0.0f,0.0f,0.0f);
        filteredPos = new Vector3(0.0f,0.0f,0.0f);
        filteredQuaternion = new Quaternion(0.0f,0.0f,0.0f,0.0f);
        for(int i=0;i<keepFrame;i++){
            posQue.Enqueue(new Vector3(0.0f,0.0f,0.0f));
            quaternionQue.Enqueue(new Quaternion(0.0f,0.0f,0.0f,0.0f));
        }
    }

    void Update () {

        Quaternion q;

        string[] splitText = udpReceiver.GetrawText().Split('_');
        //受信できてないときどうなってんのか確認
        //ずっと値が入ってる
        Debug.Log(splitText[11]);

        /****************
        めも
        返ってくる値がstaticなのでずっと前の値が残ってる
        それで下のif文が走る
        それで、UDPReceiver側で値が更新されてないとき(udp.Receiveが走ってないとき)は、
        空の文字列が返ってくるようにして、値が来てないときは下のif文が走らんようにした。
        そしたら動きががくがくになった。（当然だけど）
        動きと回転の更新は常に続ける必要がある。

        使えそうな方法:quaternionの線形補完
        Time.deltatimeでいい感じに補完する
        30fpsで値が来ることを信じる。
        0sのとき0
        1/30sのとき1
        になるように補完する
        dt = Time.deltaTime * 30;
        pre = Quaternion.Lerp(pre,now,dt);
        *****************/

        // 関節の回転を取得する
        if (splitText.Length >= receiveQuaternionNum + 1)
        {

            // 回転の初期化
            //ここ試しにコメント化してみよう
            q = transform.rotation;
            Debug.Log("unitychan" + q.ToString("f7"));
            transform.rotation = Quaternion.identity;

            //ここで飛んできた値を気合でパース
            Quaternion[] receiveQuaternion = new Quaternion[receiveQuaternionNum];
            //ここはQuaternion
            for(int i=0;i<receiveQuaternionNum;i++){
                string[] quaternionStr = splitText[i].Split(',');
                for(int j=0;j<4;j++){
                    receiveQuaternion[i][j] = float.Parse(quaternionStr[j]);
                }
            }
            //ここはunityちゃんのposition
            string[] rawPosStr = splitText[11].Split(',');
            rawPos = new Vector3(float.Parse(rawPosStr[0]),float.Parse(rawPosStr[1]),float.Parse(rawPosStr[2]));

            //ローパスフィルター処理
            /*
            Quaternion dequeueQuaternion;
            dequeueQuaternion = quaternionQue.Dequeue();
            for(int i=0;i<4;i++){
                quaternionLowPassBuffer[i] -= dequeueQuaternion[i];
                quaternionLowPassBuffer[i] += Spine1.transform.rotation[i];
                filteredQuaternion[i] = quaternionLowPassBuffer[i] / (float)keepFrame;
            }
            quaternionQue.Enqueue(receiveQuaternion[0]);
            Spine1.transform.rotation = filteredQuaternion;
            */
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
            //ここ試しにコメント化してみよう
            transform.rotation = q;

            /*
            //ローパスフィルター処理
            lowPassBuffer -= posQue.Dequeue();
            lowPassBuffer += rawPos;
            posQue.Enqueue(rawPos);
            filteredPos = lowPassBuffer / (float)keepFrame;

            //キャリブレーション関連
            if(OVRInput.Get(OVRInput.Button.PrimaryTouchpad) || Input.GetMouseButton(1)){
                touchTime += Time.deltaTime;
            }else{
                touchTime = 0.0f;
            }

            //kinectの初期値をとっとく
            //補正値初期化と補正値設定をまとめてやる
            if(touchTime > 2.0f){
                calibrationPos = new Vector3(0.0f,0.0f,0.0f);
                calibrationPos = filteredPos;
                calibrationPos = new Vector3(calibrationPos.x,calibrationPos.y - floorDistance, calibrationPos.z);
            }
            //補正後の値 = 生の値 - 補正値
            fixPos = filteredPos - calibrationPos;
            // モデルの位置を移動する
            transform.position = fixPos;
            */

        }


        //ローパスフィルター処理
        lowPassBuffer -= posQue.Dequeue();
        lowPassBuffer += rawPos;
        posQue.Enqueue(rawPos);
        filteredPos = lowPassBuffer / (float)keepFrame;

        //キャリブレーション関連
        if(OVRInput.Get(OVRInput.Button.PrimaryTouchpad) || Input.GetMouseButton(1)){
            touchTime += Time.deltaTime;
        }else{
            touchTime = 0.0f;
        }

        //kinectの初期値をとっとく
        //補正値初期化と補正値設定をまとめてやる
        if(touchTime > 2.0f){
            calibrationPos = new Vector3(0.0f,0.0f,0.0f);
            calibrationPos = filteredPos;
            calibrationPos = new Vector3(calibrationPos.x,calibrationPos.y - floorDistance, calibrationPos.z);
        }
        //補正後の値 = 生の値 - 補正値
        fixPos = filteredPos - calibrationPos;
        // モデルの位置を移動する
        transform.position = fixPos;
    }
}