using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

/*********************************
このクラスはunityちゃんを動かします
**********************************/

public class KinectAvatar : MonoBehaviour {

    //ネットワーク関連
    private UDPReceiver udpReceiver;
    private const int receiveQuaternionNum = 11;

    //キャリブレーションするときにつかう
    private Vector3 calibrationPos;
    private Vector3 rawPos;
    [SerializeField] private Transform floorPos;
    private float floorDistance;
    private float touchTime;

    //カメラ　ローパスフィルターもどき
    [SerializeField] private const int keepFrame = 10;
    //ここはposition
    Queue<Vector3> posQue = new Queue<Vector3>();
    private Vector3 lowPassBuffer,filteredPos;

    //unityちゃんの回転に使う
    Quaternion[] receiveQuaternion = new Quaternion[receiveQuaternionNum];
    [SerializeField,Range(0f,1f)] private float lerpValue = 0.5f;

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
        filteredPos = new Vector3(0.0f,0.0f,0.0f);
        for(int i=0;i<keepFrame;i++){
            posQue.Enqueue(new Vector3(0.0f,0.0f,0.0f));
        }
    }

    void Update () {

        Quaternion q;

        string[] splitText = udpReceiver.GetrawText().Split('_');

        //ここで飛んできた値を気合でパース
        if (splitText.Length >= receiveQuaternionNum + 1)
        {
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

        }

        // 回転の初期化
        q = transform.rotation;
        transform.rotation = Quaternion.identity; 

        Spine1.transform.rotation = Quaternion.Lerp(Spine1.transform.rotation,receiveQuaternion[0],0.1f);
        RightArm.transform.rotation = Quaternion.Lerp(RightArm.transform.rotation,receiveQuaternion[1],lerpValue);
        RightForeArm.transform.rotation = Quaternion.Lerp(RightForeArm.transform.rotation,receiveQuaternion[2],lerpValue);
        RightHand.transform.rotation = Quaternion.Lerp(RightHand.transform.rotation,receiveQuaternion[3],lerpValue);
        LeftArm.transform.rotation = Quaternion.Lerp(LeftArm.transform.rotation,receiveQuaternion[4],lerpValue);
        LeftForeArm.transform.rotation = Quaternion.Lerp(LeftForeArm.transform.rotation,receiveQuaternion[5],lerpValue);
        LeftHand.transform.rotation = Quaternion.Lerp(LeftHand.transform.rotation,receiveQuaternion[6],lerpValue);
        RightUpLeg.transform.rotation = Quaternion.Lerp(RightUpLeg.transform.rotation,receiveQuaternion[7],lerpValue);
        RightLeg.transform.rotation = Quaternion.Lerp(RightLeg.transform.rotation,receiveQuaternion[8],lerpValue);
        LeftUpLeg.transform.rotation = Quaternion.Lerp(LeftUpLeg.transform.rotation,receiveQuaternion[9],lerpValue);
        LeftLeg.transform.rotation = Quaternion.Lerp(LeftLeg.transform.rotation,receiveQuaternion[10],lerpValue);

        transform.rotation = q;

        //ここはVector3.Lerpで置き換え可能
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
        
        //モデルの位置を移動
        //補正後の値 = 生の値 - 補正値
        transform.position = filteredPos - calibrationPos;
    }
}