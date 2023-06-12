using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimescaleController : MonoBehaviour
{
    public static bool isFastForward = false;
    public static bool isDialogue = false;
    public GameObject fastForwardButton;
    void Awake(){
        if (true){//.GetInt("GamesFinished",0)>0){
            fastForwardButton.SetActive(true);
        }
    }
    public void SetFastForward(){
        Time.timeScale=10f;
        isFastForward=true;
        isDialogue = (DialogueController.playing);
    }
    public void UnSetFastForward(){
        Time.timeScale=1f;
        isFastForward=false;
    }
    public void OnDisable() {
        UnSetFastForward();
    }
}
