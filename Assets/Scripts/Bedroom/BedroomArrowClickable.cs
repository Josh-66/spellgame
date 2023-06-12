using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomArrowClickable : BedroomObject
{


    // Start is called before the first frame update
    void Start()
    {
        if (!BedroomController.morning){
            gameObject.SetActive(false);
        }
    }
    public override void Activate()
    {
       Utility.FadeToScene("Shop");
        
    }
}
