using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedClickable : BedroomObject
{
    public static BedClickable instance;
    public bool inactive = true;
    public override void Awake(){
        base.Awake();
        instance=this;
    }
    public override void Activate()
    {
        if (!inactive){
            if (Utility.FadeToScene("Title")){
                PlayerPrefs.SetInt("GamesFinished",PlayerPrefs.GetInt("GamesFinished",0)+1);
                SaveData.validSave=false;
            }
            
        }
    }
    public override void Update()
    {
        base.Update();
        if (inactive)
            spriteRenderer.material =  regular;
    }
}
