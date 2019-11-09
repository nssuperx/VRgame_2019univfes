using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private GameObject unityChan;
    void Start(){
        unityChan = GameObject.Find("unitychan");
    }
    void OnCollisionEnter (Collision collision) {
        unityChan.GetComponent<UnitychanMethod>().CollisionEnterMethod(collision);
    }
}
