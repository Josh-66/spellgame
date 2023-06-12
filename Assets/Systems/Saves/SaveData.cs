using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class SaveData
{
    public string stampPixels;
    public Rect stampRect;
    // public Vector3Int stampSize;
    public List<CustomerSpawnRequest> remainingCustomers;
    public List<Evaluation> evaluations;
    public static bool validSave {get{return PlayerPrefs.GetInt("ValidSave",0)==1;} set{PlayerPrefs.SetInt("ValidSave",value ? 1:0);}}

    public static void Save(){
        SaveData data = new SaveData();
        PlayerPrefs.SetInt("ValidSave",1);

        data.stampPixels=System.Convert.ToBase64String(StampPaperController.stampTexture.EncodeToPNG());
        data.stampRect=StampPaperController.stampSprite.rect;
        // data.stampSize=new Vector3Int(StampPaperController.stampTexture.width,StampPaperController.stampTexture.height,InkController.scale);

        data.evaluations = GameController.evaluations;
        data.remainingCustomers = GameController.customerQueue;

        SavingText.Display();

        SaveLoad<SaveData>.Save(data,"","data");
    }
    public static void Load(){
        SaveData data = SaveLoad<SaveData>.Load("","data");


        //InkController stampInk = StampPaperController.instance.inkController;
        // StampPaperController.stampTexture=new Texture2D(data.stampSize.x,data.stampSize.y);
        StampPaperController.stampTexture=new Texture2D(1,1);
        StampPaperController.stampTexture.LoadImage(System.Convert.FromBase64String(data.stampPixels));
        StampPaperController.stampTexture.Apply();
        StampPaperController.stampSprite=Sprite.Create(StampPaperController.stampTexture,data.stampRect,new Vector2(.5f,.5f),20);
        StampPaperController.instance.UpdatePreview();

        GameController.customerQueue=data.remainingCustomers;
        GameController.evaluations=data.evaluations;
        
        
    }
}
