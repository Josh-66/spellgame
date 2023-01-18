using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SavingText : MonoBehaviour
{
    static SavingText instance;
    bool displaying;
    float t = 0;
    TextMeshProUGUI tmp;
    void Awake()
    {
        instance=this;
        tmp = GetComponent<TextMeshProUGUI>();
        tmp.color=new Color(1,1,1,0);
    }

    // Update is called once per frame
    void Update()
    {
        if (displaying){
            t+=Time.deltaTime;
            if (t<1){
                tmp.color=new Color(1,1,1,t);
            }
            else if (t<3){
                tmp.color=new Color(1,1,1,1);
            }
            else if (t<4){
                tmp.color=new Color(1,1,1,4-t);
            }
            else{
                tmp.color=new Color(1,1,1,0);
                displaying=false;
            }
        }
    }

    public static void Display(){
        instance.displaying=true;
        instance.t=0;
    }
}
