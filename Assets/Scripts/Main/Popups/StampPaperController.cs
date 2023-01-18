using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampPaperController : WindowController
{
    public InkController inkController;
    public static StampPaperController instance;
    public StampPreviewController previewController;
    public static Texture2D stampTexture = null;
    public static Sprite stampSprite = null;
    public static bool isOpen {get{return instance.gameObject.activeSelf;}}
    
    
    public override void Activate(){
        instance=this;
        ClosePaper();
    }
    public override void Update(){
        base.Update();        
    }
    public void Clear(){
        inkController.ClearStrokes();
    }
    public void SaveStamp(){
        if (inkController.strokes.Count>0){
                stampTexture = inkController.strokes[0].texture;
                Vector3Int min =inkController.strokes[0].bounds.min;
                Vector3Int max =inkController.strokes[0].bounds.max;
                Stroke s = inkController.strokes[0];
                for(int x = s.bounds.xMin ; x<=s.bounds.xMax;x++){
                    for(int y = s.bounds.yMin ; y<=s.bounds.yMax;y++){
                        float alpha = s.texture.GetPixel(x,y).a;
                        if (alpha>0){ 
                            stampTexture.SetPixel(x,y,new Color(1,1,1,alpha));
                        }
                    }  
                }
                for (int i = 1 ; i < inkController.strokes.Count;i++){
                    s = inkController.strokes[i];
                    min=Vector3Int.Min(min,s.bounds.min);
                    max=Vector3Int.Max(max,s.bounds.max);
                    for(int x = s.bounds.xMin ; x<=s.bounds.xMax;x++){
                        for(int y = s.bounds.yMin ; y<=s.bounds.yMax;y++){
                            float alpha = s.texture.GetPixel(x,y).a;
                            if (alpha>0){ 
                                stampTexture.SetPixel(x,y,stampTexture.GetPixel(x,y)+new Color(1,1,1,alpha));
                            }
                        }  
                    }
                }
                stampTexture.Apply();
                float width =  Mathf.Ceil((max.x-min.x+1)/2)*2;
                float height = Mathf.Ceil((max.y-min.y+1)/2)*2;
                stampSprite= Sprite.Create(stampTexture,new Rect(min.x,min.y,width,height),new Vector2(.5f,.5f),20);


                inkController.HardClearStrokes();
                
                UpdatePreview();

                ClosePaper();

        }
    }
    public void UpdatePreview(){
        previewController.image.sprite=stampSprite;
        previewController.image.SetNativeSize();
        ((RectTransform)previewController.transform).sizeDelta*=InkController.scale/5f;
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