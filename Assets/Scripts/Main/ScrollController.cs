using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollController : MonoBehaviour
{
    public static ScrollController instance;
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
        ToolController.ClearTool();
        instance.source.clip=instance.pickUp;
        instance.source.Play();
        
    }
    public static void CloseScroll(bool silent = false){
        instance.gameObject.SetActive(false);
        ToolController.ClearTool();
        instance.source.clip=instance.putDown;

        if (!silent)
            instance.source.Play();

    }
    public static void ToggleScroll(){
        instance.gameObject.SetActive(!isOpen);
        ToolController.ClearTool();
    
        instance.source.clip=isOpen ? instance.pickUp : instance.putDown;
        instance.source.Play();
        
    }
    
}
