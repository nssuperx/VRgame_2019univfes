using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    private Vector3 fixPos;
    private Transform floorPos;
    private float floorDistance;
    private float touchTime;

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
        floorPos = GameObject.Find("Cube").transform;
        floorDistance = floorPos.localScale.y / 2 + floorPos.position.y;
    }

    void Update () {

        Quaternion q;

        string[] splitText = udpReceiver.GetrawText().Split('_');
        
        // 関節の回転を取得する
        if (splitText.Length > 0)
        {

            // 回転の初期化
            q = transform.rotation;
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
            //ここは位置
            string[] rawPosStr = splitText[11].Split(',');
            rawPos = new Vector3(float.Parse(rawPosStr[0]),float.Parse(rawPosStr[1]),-float.Parse(rawPosStr[2]));


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

            //キャリブレーション関連
            if(OVRInput.Get(OVRInput.Button.PrimaryTouchpad) || Input.GetMouseButton(1)){
                touchTime += Time.deltaTime;
            }else{
                touchTime = 0.0f;
            }

            //kinectの初期値をとっとく
            //補正値初期化と補正値設定をまとめてやる
            if(Input.GetMouseButtonDown(0) || touchTime > 2.0f){
                calibrationPos = new Vector3(0.0f,0.0f,0.0f);
                calibrationPos = rawPos;
                calibrationPos = new Vector3(calibrationPos.x,calibrationPos.y - floorDistance, calibrationPos.z);
            }
            //補正後の値 = 生の値 - 補正値
            fixPos = rawPos - calibrationPos;
            // モデルの位置を移動する
            transform.position = fixPos;

        }
    }
}