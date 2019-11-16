using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootBullet : MonoBehaviour
{
    /// <summary>
    /// 射出間のスパン
    /// </summary>
    [SerializeField, Range(1f, 30f)]
    private float shootSpan = 3f;

    /// <summary>
    /// 前回の射出から経過した時間
    /// </summary>
    private float waitTime = 0;

    /// <summary>
    /// 弾のPrefab
    /// </summary>
    [SerializeField, Tooltip("弾のPrefab")]
    private GameObject bulletPrefab;

    /// <summary>
    /// 砲身のオブジェクト
    /// </summary>
    [SerializeField, Tooltip("砲身のオブジェクト")]
    private GameObject barrelObject;

    /// <summary>
    /// 弾を生成する位置情報
    /// </summary>
    private Vector3 instantiatePosition;
    /// <summary>
    /// 弾の生成座標(読み取り専用)
    /// </summary>
    public Vector3 InstantiatePosition
    {
        get { return instantiatePosition; }
    }

    /// <summary>
    /// 弾の速さ
    /// </summary>
    [SerializeField, Range(1.0F, 50.0F), Tooltip("弾の射出する速さ")]
    private float speed = 15.0F;

    /// <summary>
    /// 弾の初速度
    /// </summary>
    private Vector3 shootVelocity;
    /// <summary>
    /// 弾の初速度(読み取り専用)
    /// </summary>
    public Vector3 ShootVelocity
    {
        get { return shootVelocity; }
    }

    void Update ()
    {
        // 弾の初速度を更新
        shootVelocity = barrelObject.transform.up * speed;

        // 弾の生成座標を更新
        instantiatePosition = barrelObject.transform.position; 

        // 発射
        if (waitTime > shootSpan)
        {
            // 弾を生成して飛ばす
            GameObject obj = Instantiate(bulletPrefab, instantiatePosition, Quaternion.identity);
            Rigidbody rid = obj.GetComponent<Rigidbody>();
            rid.AddForce(shootVelocity * rid.mass, ForceMode.Impulse);

            // 5秒後に消える
            Destroy(obj, 5.0F);

            waitTime = 0; // 初期化
        }
        waitTime += Time.deltaTime;
    }
}
