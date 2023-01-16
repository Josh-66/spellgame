using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampPaperController : WindowController
{
    public InkController inkController;
    public static StampPaperController instance;
    public StampPreviewController previewController;
    public Texture2D stampTexture = null;
    public Sprite stampSprite = null;
    public static bool isOpen {get{return instance.gameObject.activeSelf;}}
    public override void Activate(){
        instance=this;
        ClosePaper();
    }
    public override void Update(){
        base.Update();
        if (Input.GetKeyDown(KeyCode.R)){
            Clear();
        }
        if (Input.GetKeyDown(KeyCode.Return)){
            SaveStamp();
        }
    }
    public void Clear(){
        inkController.ClearStrokes();
    }
    public void SaveStamp(){
        if (inkController.strokes.Count>0){
                stampTexture = inkController.strokes[0].texture;
                Vector3Int min =inkController.strokes[0].bounds.min;
                Vector3Int max =inkController.strokes[0].bounds.max;
                for (int i = 1 ; i < inkController.strokes.Count;i++){
                    Stroke s = inkController.strokes[i];
                    min=Vector3Int.Min(min,s.bounds.min);
                    max=Vector3Int.Max(max,s.bounds.max);
                    for(int x = s.bounds.xMin ; x<=s.bounds.xMax;x++){
                        for(int y = s.bounds.yMin ; y<=s.bounds.yMax;y++){
                            if (s.texture.GetPixel(x,y).a>stampTexture.GetPixel(x,y).a)
                                stampTexture.SetPixel(x,y,s.texture.GetPixel(x,y));
                        }  
                    }
                }
                stampTexture.Apply();
                float width =  Mathf.Floor((max.x-min.x+1)/2)*2;
                float height = Mathf.Floor((max.y-min.y+1)/2)*2;
                stampSprite= Sprite.Create(stampTexture,new Rect(min.x,min.y,width,height),new Vector2(.5f,.5f),20);

                inkController.ClearStrokes();
                previewController.image.sprite=stampSprite;
                previewController.image.SetNativeSize();
                ClosePaper();

        }
    }
    public static void OpenPaper(){
        instance.Open();

        ToolController.activeTool=Tool.StampQuill;
        GameController.instance.clickablesActive=false;

        
    }
    public static void ClosePaper(bool silent = false){
        instance.Close(silent);
        ToolController.activeTool=Tool.None;
        GameController.instance.clickablesActive=true;
    }
    public static void TogglePaper(){
        if (isOpen){
            ClosePaper();
        }
        else{
            OpenPaper();
        }
        
    }
}