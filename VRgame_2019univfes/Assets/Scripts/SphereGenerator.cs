using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SphereGenerator : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject spherePrefab;
    float span = 0.3f;
    float delta = 0;

    //GameObject SpherePrefab; //SpherePrefab内のオブジェクトそのものが入る変数
    //GameObject.Instantiate(SpherePrefab);
    //SphereGenerator spherescript;//SphereConntrollerというスクリプトが入る変数

    void Start()
    {
        //SpherePrefab = GameObject.Find("SpherePrefab");
        //spherescript = SpherePrefab.GetComponent<SphereController>();
    }
    // Update is called once per frame
    void Update()
    {
        this.delta += Time.deltaTime;
        if(this.delta > this.span)
        {
            this.delta = 0;

            GameObject go = Instantiate(spherePrefab) as GameObject;
            
            float xpos = Random.Range(-0.4f,0.4f);
            //float xpos = 0.5f;
            //int zpos = Random.Range(-25,25);
            float ypos = Random.Range(0,0.4f);
            go.transform.position = this.transform.TransformPoint(xpos,ypos,1);

            Vector3 dir = this.transform.forward;
            Vector3 force = dir * 500;
            go.GetComponent<Rigidbody>().AddForce(force);
           
           
        }
    }
}
