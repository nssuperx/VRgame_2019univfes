using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereController : MonoBehaviour
{
    // Start is called before the first frame update
    //GameObject Cube;
    //float xwidth;
    //float zwidth;

    Vector3 firstpos;
   
    void Start()
    {
        //this.Cube =GameObject.Find("Cube");
        //xwidth = Cube.GetComponent<Renderer>().bounds.size.x;
        //zwidth = Cube.GetComponent<Renderer>().bounds.size.z;
        firstpos = this.transform.position;  //最初の位置を記憶させる
    }

    // Update is called once per frame
    void Update()
    {
        //　～したら、ボールを消す
        Vector3 Balldistance = this.transform.position - firstpos;
        if(Mathf.Abs(Balldistance.z)  > 50)
        {
            Destroy(gameObject);
        }
        if(Mathf.Abs(Balldistance.x) > 50)
        {
            Destroy(gameObject);
        }
    }
}
