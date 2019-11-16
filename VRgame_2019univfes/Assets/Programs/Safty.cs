using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Safty : MonoBehaviour
{
   
    private float difposx;

    private float difposz;

    public Text difposX;

    public Text difposZ;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        difposx = this.transform.position.x;
        difposz = this.transform.position.z;
        difposX.text = "difposx:" + difposx.ToString();
        difposZ.text = "difposz:" + difposz.ToString();
        
    }

    
}
