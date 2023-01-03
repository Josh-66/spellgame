using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampPaperController : MonoBehaviour
{
    public InkController inkController;
    public static StampPaperController instance;
    public AudioSource source;
    public AudioClip openSound,closeSound;
    public static bool isOpen {get{return instance.gameObject.activeSelf;}}
    public void Awake(){
        instance=this;
        ClosePaper();
    }

    public static void OpenPaper(){
        instance.gameObject.SetActive(true);
        ToolController.activeTool=Tool.StampQuill;
        instance.source.clip=instance.openSound;
        instance.source.Play();

        GameController.instance.clickablesActive=false;
        
    }
    public static void ClosePaper(bool silent = false){
        instance.gameObject.SetActive(false);
        instance.source.clip=instance.closeSound;

        if (!silent)
            instance.source.Play();

    }
    public static void TogglePaper(){
        instance.gameObject.SetActive(!isOpen);
        ToolController.ClearTool();
    
        instance.source.clip=isOpen ? instance.openSound : instance.closeSound;
        
        GameController.instance.clickablesActive=!isOpen;
        instance.source.Play();
        
    }
}