using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour
{
    public static ScrollController instance;
    public static Color inkColor {get{return instance.inkController.inkColor;}}
    public InkController inkController;

    public AudioSource source;
    public AudioClip pickUp,putDown;
    public static bool isOpen {get{return instance.gameObject.activeSelf;}}
    void Awake(){
        instance=this;
        CloseScroll(true);        
    }


    void Update(){

    }

    public static void OpenScroll(){
        instance.gameObject.SetActive(true);
        instance.source.clip=instance.pickUp;
        instance.source.Play();
        
    }
    public static void CloseScroll(bool silent = false){
        instance.gameObject.SetActive(false);
        instance.source.clip=instance.putDown;

        if (!silent)
            instance.source.Play();

    }
    public static void ToggleScroll(){
        instance.gameObject.SetActive(!isOpen);
    
        instance.source.clip=isOpen ? instance.pickUp : instance.putDown;
        instance.source.Play();
        
    }
    
}
