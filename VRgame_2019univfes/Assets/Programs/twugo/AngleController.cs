using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleController : MonoBehaviour
{
    /// <summary>
    /// 感度
    /// </summary>
    [SerializeField, Range(0.01F, 5.0F), Tooltip("感度")]
    private float sensitivity = 1.0F;

    /// <summary>
    /// 砲身のオブジェクト
    /// </summary>
    [SerializeField, Tooltip("砲身のオブジェクト")]
    private GameObject barrelObject;

    void Update ()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            this.transform.Rotate(new Vector3(0F, -1.0F * sensitivity, 0F));
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            this.transform.Rotate(new Vector3(0F, 1.0F * sensitivity, 0F));
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            barrelObject.transform.Rotate(new Vector3(-1.0F * sensitivity, 0F, 0F));
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            barrelObject.transform.Rotate(new Vector3(1.0F * sensitivity, 0F, 0F));
        }

    }
}

