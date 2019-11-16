using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AimShooter : MonoBehaviour
{
    /// <summary>射出間の時間</summary>
    [SerializeField, Range(1,30)] private float shootSpan = 5F;

    /// <summary>射出待ち時間</summary>
    private float waitTime;

    /// <summary>弾が場に残る時間</summary>
    [SerializeField, Range(1,100)] private float remainTime = 10f;
    public float RemainTime{
        get{return remainTime;}
        private set{remainTime = value;}
    }

    private bool isBallShoot = false;
    public bool IsBallShoot{
        get{return isBallShoot;}
        private set{isBallShoot = value;}
    }

    private Vector3 shootVelocity;
    public Vector3 ShootVelocity{
        get{return shootVelocity;}
        private set{shootVelocity = value;}
    }

    private float ballMass;

    /// <summary>
    /// 射出するオブジェクト
    /// </summary>
    [SerializeField, Tooltip("射出するオブジェクトをここに割り当てる")]
    private GameObject ThrowingObject;

    /// <summary>
    /// 標的のオブジェクト
    /// </summary>
    [SerializeField, Tooltip("標的のオブジェクトをここに割り当てる")]
    private GameObject TargetObject;

    /// <summary>
    /// 射出角度
    /// </summary>
    [SerializeField, Range(0F, 90F), Tooltip("射出する角度")]
    private float ThrowingAngle;

    private void Start()
    {
        Collider collider = GetComponent<Collider>();
        if (collider != null)
        {
            // 干渉しないようにisTriggerをつける
            collider.isTrigger = true;
        }
        ballMass = ThrowingObject.GetComponent<Rigidbody>().mass;
    }

    private void Update()
    {
        if (waitTime > shootSpan)
        {
            // ボールを射出する
            ThrowingBall();
            
            IsBallShoot = true;
            waitTime = 0f;
        }else{
            IsBallShoot = false;
        }
        waitTime += Time.deltaTime;
        shootVelocity = CalculateVelocity(this.transform.position, TargetObject.transform.position, ThrowingAngle) * ballMass;
    }

    /// <summary>
    /// ボールを射出する
    /// </summary>
    private void ThrowingBall()
    {
        if (ThrowingObject != null && TargetObject != null)
        {
            // Ballオブジェクトの生成
            GameObject ball = Instantiate(ThrowingObject, this.transform.position, Quaternion.identity);

            // 標的の座標
            Vector3 targetPosition = TargetObject.transform.position;

            // 射出角度
            float angle = ThrowingAngle;

            // 射出速度を算出
            Vector3 velocity = CalculateVelocity(this.transform.position, targetPosition, angle);

            // 射出
            Rigidbody rid = ball.GetComponent<Rigidbody>();
            rid.AddForce(velocity * rid.mass, ForceMode.Impulse);
        }
        else
        {
            throw new System.Exception("射出するオブジェクトまたは標的のオブジェクトが未設定です。");
        }
    }

    /// <summary>
    /// 標的に命中する射出速度の計算
    /// </summary>
    /// <param name="pointA">射出開始座標</param>
    /// <param name="pointB">標的の座標</param>
    /// <returns>射出速度</returns>
    private Vector3 CalculateVelocity(Vector3 pointA, Vector3 pointB, float angle)
    {
        // 射出角をラジアンに変換
        float rad = angle * Mathf.PI / 180;

        // 水平方向の距離x
        float x = Vector2.Distance(new Vector2(pointA.x, pointA.z), new Vector2(pointB.x, pointB.z));

        // 垂直方向の距離y
        float y = pointA.y - pointB.y;

        // 斜方投射の公式を初速度について解く
        float speed = Mathf.Sqrt(-Physics.gravity.y * Mathf.Pow(x, 2) / (2 * Mathf.Pow(Mathf.Cos(rad), 2) * (x * Mathf.Tan(rad) + y)));

        if (float.IsNaN(speed))
        {
            // 条件を満たす初速を算出できなければVector3.zeroを返す
            return Vector3.zero;
        }
        else
        {
            return (new Vector3(pointB.x - pointA.x, x * Mathf.Tan(rad), pointB.z - pointA.z).normalized * speed);
        }
    }
}