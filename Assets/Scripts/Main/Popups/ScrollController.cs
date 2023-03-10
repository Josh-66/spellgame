using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ScrollController : WindowController
{
    public static ScrollController instance;
    public static Color inkColor {get{return instance.inkController.inkColor;}}
    public InkController inkController;
    public static bool isOpen {get{return instance.gameObject.activeSelf;}}

    public static void OpenScroll(){
        instance.Open();
        
    }
    public static void CloseScroll(bool silent = false){
        foreach(PixController pix in instance.GetComponentsInChildren<PixController>()){
            GameObject.Destroy(pix.gameObject);
        }
        instance.Close(silent);
    }
    public static void ToggleScroll(){
        foreach(PixController pix in instance.GetComponentsInChildren<PixController>()){
            GameObject.Destroy(pix.gameObject);
        }
        instance.Toggle();
    }
    public override void Activate()
    {
        instance=this;
        CloseScroll(true);   
    }
    // public override void OnPointerDown(PointerEventData ped)
    // {
    //     //Do nothing;
    // }
}
