using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
public class BlackFade : MonoBehaviour
{
    public static BlackFade instance;
    public float targFade = 0;
    Image image;
    public Action OnReachTarg;
    void Awake(){
        instance=this;

        image = GetComponent<Image>();
        image.color=new Color(1,1,1,1);
    }
    void Update(){
        float alpha = image.color.a;
        if (alpha==targFade)
            return;
        alpha = Mathf.MoveTowards(alpha,targFade,Time.deltaTime/1.5f);
        image.color = new Color(1,1,1,alpha);
        if (alpha==targFade && OnReachTarg!=null){
            OnReachTarg();
            OnReachTarg=null;
        }
        
    }

    public static void FadeInAndAcion(Action action){
        instance.OnReachTarg=action;
        instance.targFade=1;
    }
}
