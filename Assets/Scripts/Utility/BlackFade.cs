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
    public AudioSource musicPlayer;
    void Awake(){
        instance=this;

        image = GetComponent<Image>();
        image.color=new Color(1,1,1,1);
        image.enabled=true;
    }
    void Update(){
        if (targFade==1){
            image.raycastTarget=true;
        }else{
            image.raycastTarget=false;
        }

        float alpha = image.color.a;
        if (alpha==targFade)
            return;
        alpha = Mathf.MoveTowards(alpha,targFade,Time.deltaTime/2f);
        image.color = new Color(1,1,1,alpha);
        if (musicPlayer!=null)
            musicPlayer.volume=1-alpha;
        if (alpha==targFade && OnReachTarg!=null){
            OnReachTarg();
            OnReachTarg=null;
        }
        
    }

    public static void FadeInAndAcion(Action action){
        instance.targFade=1;
        instance.OnReachTarg=action;
    }
}
