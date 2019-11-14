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

    //unityちゃんの移動の補間に使う
    private Vector3 lerpPos;
    private Vector3 defaultPos;
    [SerializeField,Range(0f,1f)] private float posLerpRate = 0.4f;

    //unityちゃんの回転と補間に使う
    Quaternion[] receiveQuaternion = new Quaternion[receiveQuaternionNum];
    [SerializeField,Range(0f,1f)] private float quatLerpRate = 0.4f;

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

        //位置と回転の初期化
        lerpPos = this.transform.position;
        defaultPos = this.transform.position;
        
        for(int i=0;i<receiveQuaternionNum;i++){
            receiveQuaternion[i] = Quaternion.identity;
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

        //回転の補間（ほんとは時間とって均等にやらないといけない）
        Spine1.transform.rotation = Quaternion.Lerp(Spine1.transform.rotation,receiveQuaternion[0],0.1f);
        RightArm.transform.rotation = Quaternion.Lerp(RightArm.transform.rotation,receiveQuaternion[1],quatLerpRate);
        RightForeArm.transform.rotation = Quaternion.Lerp(RightForeArm.transform.rotation,receiveQuaternion[2],quatLerpRate);
        RightHand.transform.rotation = Quaternion.Lerp(RightHand.transform.rotation,receiveQuaternion[3],quatLerpRate);
        LeftArm.transform.rotation = Quaternion.Lerp(LeftArm.transform.rotation,receiveQuaternion[4],quatLerpRate);
        LeftForeArm.transform.rotation = Quaternion.Lerp(LeftForeArm.transform.rotation,receiveQuaternion[5],quatLerpRate);
        LeftHand.transform.rotation = Quaternion.Lerp(LeftHand.transform.rotation,receiveQuaternion[6],quatLerpRate);
        RightUpLeg.transform.rotation = Quaternion.Lerp(RightUpLeg.transform.rotation,receiveQuaternion[7],quatLerpRate);
        RightLeg.transform.rotation = Quaternion.Lerp(RightLeg.transform.rotation,receiveQuaternion[8],quatLerpRate);
        LeftUpLeg.transform.rotation = Quaternion.Lerp(LeftUpLeg.transform.rotation,receiveQuaternion[9],quatLerpRate);
        LeftLeg.transform.rotation = Quaternion.Lerp(LeftLeg.transform.rotation,receiveQuaternion[10],quatLerpRate);

        transform.rotation = q;

        //位置の補間（がくがくしないように）
        lerpPos = Vector3.Lerp(lerpPos,rawPos + defaultPos,posLerpRate);

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
            calibrationPos = lerpPos;
            calibrationPos = new Vector3(calibrationPos.x,calibrationPos.y - floorDistance, calibrationPos.z);
        }
        
        //モデルの位置を移動
        //補正後の値 = 位置補間後の値 - 補正値
        transform.position = lerpPos - calibrationPos + defaultPos;
    }
}