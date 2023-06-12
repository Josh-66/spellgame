using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpController : MonoBehaviour
{
    public GameObject helpVisuals;
    public HelpObjectController[] objects;

    public void Awake(){
        foreach(HelpObjectController o in objects){
            o.InitializePosition();
        }
        helpVisuals.SetActive(false);
    }
    public void ShowHelp(){
        helpVisuals.SetActive(true);
        foreach(HelpObjectController o in objects){
            o.SetPosition();
        }
    } 
    public void HideHelp(){
        helpVisuals.SetActive(false);
    }
}
