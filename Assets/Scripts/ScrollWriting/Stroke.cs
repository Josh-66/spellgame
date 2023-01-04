using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Stroke : MonoBehaviour
{
    public Texture2D texture;
    public Glyph glyph,matchedGlyph;
    public GlyphType type=GlyphType.Invalid;
    public BoundsInt bounds;
    public float scale;
    public bool isStamp=false;
    public RectTransform rectTransform;

    GameObject strokeOnTable;

    public void Awake(){
        rectTransform=GetComponent<RectTransform>();

        if (ToolController.activeTool!=Tool.StampQuill)
            strokeOnTable= ScrollClickable.instance.AddStroke(GetComponent<Image>().sprite);
    }
    public void Disappear(){
        StartCoroutine("DisappearCR");
    }
    public IEnumerator DisappearCR(){
        int lastY = bounds.yMax;
        for (int y = bounds.yMax; y >= bounds.yMin ;y--){
            float pixelsInRow=0;
            for (int x = bounds.xMin; x < bounds.xMax ;x++){
                if (texture.GetPixel(x,y).a>0)
                    pixelsInRow++;
            }
            float modifier = Mathf.Lerp(1,.05f,pixelsInRow/100f);
            
            for (int x = bounds.xMin; x < bounds.xMax ;x++){
                Color c = texture.GetPixel(x,y);
                if (c.a==0)
                    continue;

                texture.SetPixel(x,y,Color.clear);
                if (Random.value>.5f*modifier){
                    continue;
                }


                GameObject g = Prefabs.Load("Pix");
                RectTransform rt = (RectTransform)g.transform;
                rt.SetParent(transform.parent.parent);
                rt.anchoredPosition=new Vector2(x,y);
                rt.anchoredPosition*=scale;
                rt.anchoredPosition-=rectTransform.sizeDelta/2;
                rt.anchoredPosition+=Vector2.one*scale/2;
                rt.sizeDelta=scale*Vector2.one;
                rt.localScale=Vector3.one;
                g.GetComponent<Image>().color=c;
                texture.SetPixel(x,y,Color.clear);

            }
            if (lastY-y>Time.deltaTime*200){
                texture.Apply();
                lastY=y;
                yield return null;
            }
        }       
        GameObject.Destroy(strokeOnTable);
        GameObject.Destroy(gameObject);
        
    }
    public void ChangeColor(Color newColor){
        StartCoroutine("ChangeColorCR",newColor);
        
    }
    public IEnumerator ChangeColorCR(Color newColor){
        int lastY = bounds.yMax;
        for (int y = bounds.yMax; y >= bounds.yMin ;y--){
            for (int x = bounds.xMin; x <= bounds.xMax ;x++){
                Color c = texture.GetPixel(x,y);
                if (c.a==0)
                    continue;
                c.r = newColor.r;
                c.g = newColor.g;
                c.b = newColor.b;

                texture.SetPixel(x,y,c);

            }

            if (lastY-y>Time.deltaTime*200)
            {
                //Fuzz the edge
                for (int x = bounds.xMin; x <= bounds.xMax ;x++){
                    Color c = texture.GetPixel(x,y-1);
                    if (Random.value<.5f)
                    {
                        if (c.a==0)
                            continue;
                        c.r = newColor.r;
                        c.g = newColor.g;
                        c.b = newColor.b;

                        texture.SetPixel(x,y-1,c);
                    }
                    if (Random.value<.4f)
                    {
                        c = texture.GetPixel(x,y-2);
                        if (c.a==0)
                            continue;
                        c.r = newColor.r;
                        c.g = newColor.g;
                        c.b = newColor.b;

                        texture.SetPixel(x,y-2,c);
                    }

                }
                texture.Apply();
                lastY=y;
                yield return null;
            }
        }       
        texture.Apply();
    }
    public bool CalculateBoxes(bool setGlyph=true){
        BoundsInt strokeBounds = new BoundsInt(0,0,0,0,0,0);
        glyph=ScriptableObject.CreateInstance<Glyph>();
        bool pixelExists=false;
        //Get Bounds of stroke in texture
        Vector3Int min= new Vector3Int(texture.width,texture.height);
        Vector3Int max = new Vector3Int(0,0);
        for(int x = 0 ; x < texture.width;x++){
            for (int y = 0 ; y < texture.height;y++){
                if (texture.GetPixel(x,y).a>0){
                    min.x = Mathf.Min(x,min.x);
                    max.x = Mathf.Max(x,max.x);
                    min.y = Mathf.Min(y,min.y);
                    max.y = Mathf.Max(y,max.y);
                    pixelExists=true;
                }
            }
        }
        if (!pixelExists){
            return false;
        }

        strokeBounds.SetMinMax(min,max);
        
        bounds=strokeBounds;
        if (!setGlyph) 
            return true;
        //Save actual size before squaring
        Vector3Int originalSize = strokeBounds.size; 

        //Square up the size
        // if (strokeBounds.size.x < strokeBounds.size.y){
        //     strokeBounds.size=new Vector3Int(strokeBounds.size.y,strokeBounds.size.y);
        // }
        // if (strokeBounds.size.x > strokeBounds.size.y){
        //     strokeBounds.size=new Vector3Int(strokeBounds.size.x,strokeBounds.size.x);
        // }
        
        //Find filled in boxes
        float stepSize = 1f/Glyph.size;
        for (int by = 0 ; by < Glyph.size; by+=1){
            for(int bx = 0 ; bx < Glyph.size; bx+=1){
                float tx = (bx/1f/Glyph.size);
                float ty = (by/1f/Glyph.size);
                BoundsInt boxBounds = new BoundsInt();

                Vector3Int boxMin = Vector3Int.FloorToInt(new Vector3(Mathf.Lerp(strokeBounds.xMin,strokeBounds.xMax,tx),         Mathf.Lerp(strokeBounds.yMin,strokeBounds.yMax,ty)));
                Vector3Int boxMax = Vector3Int.FloorToInt(new Vector3(Mathf.Lerp(strokeBounds.xMin,strokeBounds.xMax,tx+stepSize),Mathf.Lerp(strokeBounds.yMin,strokeBounds.yMax,ty+stepSize)));

               
                boxBounds.SetMinMax(boxMin,boxMax);

                for (int x = boxBounds.xMin; x<=boxBounds.xMax;x++){
                    for (int y = boxBounds.yMin; y<=boxBounds.yMax;y++){
                        Vector2Int pos = new Vector2Int(x,y);
                        if (texture.GetPixel(pos.x,pos.y).a==1){
                            glyph[bx,by]=true;
                            
                            y=boxBounds.yMax;
                            x=boxBounds.xMax;
                            break; 

                        }
                    
                    }  
                }
                    
            }
        }

        //Reset size
        strokeBounds.size=originalSize;
        //Save bounds
        bounds=strokeBounds;

        return true;
    }
    public void SetType(){

        float highScore = 0;
        foreach(Glyph g in Glyph.glyphs){
            if (g.type==type)
                continue;
            try{
                float score = g.Compare(glyph);
                // Debug.Log($"{g.type}:{score} || {g.count}:{glyph.count}");
                if (score>highScore){
                    type=g.type;
                    highScore=score;
                    matchedGlyph=g;
                }
            }
            catch{

            }
        }

        if (highScore<60){
            //Debug.Log($"Invalid Glyph::{type} with score of {highScore}");
            type=GlyphType.Invalid;
        }
        else{
//            Debug.Log($"Glyph::{type} with score of {highScore}");
        }
    }

}
