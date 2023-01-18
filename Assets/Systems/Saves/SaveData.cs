using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


[Serializable]
public class SaveData
{
    public Color[] stampPixels;
    public Rect stampRect;
    public Vector3Int stampSize;
    public List<CustomerSpawnRequest> remainingCustomers;
    public List<Evaluation> evaluations;


    public static void Save(){
        SaveData data = new SaveData();
        
        data.stampPixels=StampPaperController.stampTexture.GetPixels();
        data.stampRect=StampPaperController.stampSprite.rect;
        data.stampSize=new Vector3Int(StampPaperController.stampTexture.width,StampPaperController.stampTexture.height,InkController.scale);

        data.evaluations = GameController.instance.evaluations;
        data.remainingCustomers = GameController.instance.customerQueue;

        SavingText.Display();

        SaveLoad<SaveData>.Save(data,"","data");
    }
    public static void Load(){
        SaveData data = SaveLoad<SaveData>.Load("","data");


        InkController stampInk = StampPaperController.instance.inkController;
        StampPaperController.stampTexture=new Texture2D(data.stampSize.x,data.stampSize.y);
        StampPaperController.stampTexture.SetPixels(data.stampPixels);
        StampPaperController.stampTexture.Apply();
        StampPaperController.stampSprite=Sprite.Create(StampPaperController.stampTexture,data.stampRect,new Vector2(.5f,.5f),20);
        StampPaperController.instance.UpdatePreview();

        GameController.instance.customerQueue=data.remainingCustomers;
        GameController.instance.evaluations=data.evaluations;
        
        
    }
}
