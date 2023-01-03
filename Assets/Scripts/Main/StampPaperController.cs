using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StampPaperController : MonoBehaviour
{
    public InkController inkController;
    public static StampPaperController instance;
    public AudioSource source;
    public AudioClip openSound,closeSound;
    public Texture2D stampTexture = null;
    public Sprite stampSprite = null;
    public static bool isOpen {get{return instance.gameObject.activeSelf;}}
    public void Awake(){
        instance=this;
        ClosePaper();
    }
    void Update(){
        if (Input.GetKeyDown(KeyCode.R)){
            inkController.ClearStrokes();
        }
        if (Input.GetKeyDown(KeyCode.Return)){
            if (inkController.strokes.Count>0){
                Vector3Int min = Vector3Int.one*int.MaxValue;
                Vector3Int max = Vector3Int.one*int.MinValue;

                stampTexture = inkController.strokes[0].texture;
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
                stampSprite= Sprite.Create(stampTexture,new Rect(min.x,min.y,max.x-min.x,max.y-min.y),new Vector2(.5f,.5f),20);
                inkController.ClearStrokes();
                StampPreviewController.instance.image.sprite=stampSprite;
                StampPreviewController.instance.image.SetNativeSize();
                ClosePaper();

            }
            
        }
    }
    public static void OpenPaper(){
        instance.gameObject.SetActive(true);
        ToolController.activeTool=Tool.StampQuill;
        instance.source.clip=instance.openSound;
        instance.source.Play();

        ToolController.activeTool=Tool.StampQuill;
        GameController.instance.clickablesActive=false;

        
    }
    public static void ClosePaper(bool silent = false){
        instance.gameObject.SetActive(false);
        instance.source.clip=instance.closeSound;

        if (!silent)
            instance.source.Play();

        ToolController.activeTool=Tool.None;
        
        GameController.instance.clickablesActive=true;
        

    }
    public static void TogglePaper(){
        if (isOpen)
            ClosePaper();
        else 
            OpenPaper();
        
    }
}