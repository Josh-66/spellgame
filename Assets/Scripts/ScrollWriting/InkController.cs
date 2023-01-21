using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class InkController : MonoBehaviour,IPointerDownHandler
{
    
    public Tool tool {get{return ToolController.activeTool;}}
    public List<Stroke> strokes;
    Stroke activeStroke;
    public int width = 700, height = 800;
    public float border;
    public static int scale=4;
    Vector2 lastMousePos;
    Vector2 mousePos{get{

        Vector2 localMousePosition = MyInput.mousePosition;
        //localMousePosition = Camera.main.ScreenToWorldPoint(localMousePosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(transform,localMousePosition,Camera.main,out localMousePosition);
        //localMousePosition-=(Vector2)((RectTransform)transform).position;

        // localMousePosition.x/=CanvasController.transform.localScale.x;
        // localMousePosition.y/=CanvasController.transform.localScale.y;
        localMousePosition+=new Vector2(width,height)/2;
        localMousePosition/=scale;


        return localMousePosition;
    }}

    public Sprite grid,gridsquare; 
    public Material inkMaterial;
    Color baseColor ;
    public Color inkColor= new Color(.18f,.18f,.18f,1);

    public Spell spell;
    public bool isStampPad = false;
    public bool stamped=false;
    public GlyphType lastGlyph;
    public float particleTimer=1;
    new RectTransform transform {get{return (RectTransform)base.transform;}}

    public AudioClip[] shortStrokes,longStrokes;
    public AudioSource audioSource;
    public float audioCooldown = 0;

    public float brushTilt = 0;
    bool erasing = false;
    bool dragging = false;


    Vector2 dragOffset;
    void Awake()
    {
        baseColor=inkColor;

        strokes=new List<Stroke>();
        RectTransform rt = GetComponent<RectTransform>();
        width =  Mathf.FloorToInt(rt.sizeDelta.x);
        height = Mathf.FloorToInt(rt.sizeDelta.y);

        spell= new Spell();
    }
    void Update()
    {
        ControlDrag();
        if (erasing){
            EraseSpot();
            if (MyInput.clickUp)
                erasing=false;
        }
        if (activeStroke==null)
            brushTilt=0;
        if (activeStroke!=null){
            DrawStroke();
        }
        if (MyInput.clickUp && activeStroke!=null){
            EndStroke();
        }


        

        if (spell.element!=GlyphType.Invalid && ToolController.activeTool!=Tool.StampQuill){
            particleTimer-=Time.deltaTime;
            if (particleTimer<0)
            {
                particleTimer=Random.Range(.2f,.8f);
                SpawnParticles(Random.Range(2,10));
            }
        }
        else{
            particleTimer=1f;
        }
        
    }
    public void ClearStrokes(){
        for (int i = strokes.Count-1; i >= 0;i--){
            DeleteStroke(strokes[i],true);
        } 
        strokes.Clear();
    } 
    public void HardClearStrokes(){
        for (int i = strokes.Count-1; i >= 0;i--){
            strokes[i].HardDelete();
        } 
        spell=new Spell();
        UpdateInkColor();
        stamped=false;
        strokes.Clear();
    }   
    Stroke CreateStamp(){
        Stroke s = CreateStroke();
        activeStroke=null;
        Sprite stampSprite = StampPaperController.stampSprite;
        Rect spriteRect = stampSprite.rect;
        Texture2D stampTexture = StampPaperController.stampTexture;
        Vector2 previewPos = (StampPreviewController.instance.transform.anchoredPosition+transform.sizeDelta/2)/InkController.scale;
        Vector2Int localPreviewPos= Vector2Int.FloorToInt(previewPos-stampSprite.pivot-spriteRect.min); 
    
        for (int y = Mathf.FloorToInt(spriteRect.yMin);y<=spriteRect.yMax;y++){
            for (int x = Mathf.FloorToInt(spriteRect.xMin);x<=spriteRect.xMax;x++){
                Color stampColor = stampTexture.GetPixel(x,y);      
                if (stampColor.a==0)
                    continue;
                int sx = x+localPreviewPos.x;   
                int sy = y+localPreviewPos.y;

                s.texture.SetPixel(sx,sy,stampColor * baseColor);   
                
                
                if (Random.value<.2f){
                    Vector2 position=new Vector2(sx,sy);
                    position*=scale;
                    position-=transform.sizeDelta/2;
                    position+=Vector2.one*scale/2;
                    PixController pix = PixController.CreatePix(transform.parent,position+transform.anchoredPosition,scale*3,inkColor);
                    pix.velocity=new Vector2(x,y)-stampSprite.pivot;
                }



            }
        }
        s.texture.Apply();
        s.isStamp=true;
        s.CalculateBoxes(false);
        s.ChangeColor(inkColor);
        stamped=true;
        return s;

    }

    Stroke CreateStroke(){
        //Create new Stroke
        GameObject g = new GameObject($"Stroke{strokes.Count}");
        g.AddComponent<RectTransform>();
        g.transform.SetParent(transform);
        ((RectTransform)g.transform).anchoredPosition3D=Vector3.zero;
        ((RectTransform)g.transform).localScale=Vector3.one;
        g.transform.localRotation=Quaternion.identity;
        Image image = g.AddComponent<Image>();
        image.raycastTarget=false;
        //Initialize Texture
        Texture2D texture = new Texture2D(width/scale,height/scale);
        texture.filterMode=FilterMode.Point;
        var colors = texture.GetPixels();
        for (int i = 0 ; i < colors.Length;i++)
            colors[i]=Color.clear;
        texture.SetPixels(0,0,width/scale,height/scale,colors);
        texture.Apply();

        //Make and initialize sprite
        image.sprite=Sprite.Create(texture,new Rect(0,0,width/scale,height/scale),new Vector2(.5f,.5f),20);
        image.SetNativeSize();
        ((RectTransform)image.transform).sizeDelta *=scale/5f;
        image.material=inkMaterial;

        


        //Update variables
        activeStroke=g.AddComponent<Stroke>();
        activeStroke.texture=texture;
        activeStroke.scale=scale;
        strokes.Add(activeStroke);
        audioCooldown=0;


        lastMousePos=mousePos;

        return activeStroke;
    }
    void EndStroke(){

        bool exists = activeStroke.CalculateBoxes();

        audioSource.Stop();

        if (!exists){
            DeleteStroke(activeStroke);
            activeStroke=null;
            return;
        }
        if (ToolController.activeTool!=Tool.StampQuill){
            activeStroke.SetType();
            ActivateStroke(activeStroke);

            Vector3 textPosition = activeStroke.bounds.max*scale + activeStroke.transform.position - (Vector3)((RectTransform)activeStroke.transform).sizeDelta/2 + Vector3.up*40;
            PopupText.Create(inkColor,
                            transform,
                            textPosition,
                            activeStroke.type.ToString()); 
        }

        if (tool==Tool.Debug)
            DrawBoxOnStroke();
        activeStroke=null;
    }

    void DrawStroke(){
        //Get mouse position in texture coordinates
        Vector2 localMousePosition = mousePos;
        if (localMousePosition!=lastMousePos){
            brushTilt+=localMousePosition.x-lastMousePos.x;
            brushTilt = Mathf.Clamp(brushTilt,0,35);

            //Draw new pixels spaced along stroke
            Texture2D texture = activeStroke.texture;

            Color strokeColor=inkColor;
            float dist = (Vector2.Distance(localMousePosition,lastMousePos));
            audioCooldown-=Time.deltaTime;

            if (audioCooldown<0 && !audioSource.isPlaying){
                // audioSource.clip = (dist/Time.deltaTime) switch{
                //     <50f => shortStrokes.RandomElement<AudioClip>(),
                //     _ => longStrokes.RandomElement<AudioClip>(),
                // };
                audioSource.clip=longStrokes.RandomElement<AudioClip>();
                audioCooldown=Random.Range(.25f,.75f);
                audioSource.Play();
            }

            for (float t = 0 ; t <= 1 ; t+=1f/dist){
                Vector2 lerpPosition = Vector2.Lerp(lastMousePos,localMousePosition,t);

                Vector2Int pixelPosition=new Vector2Int((Mathf.FloorToInt(lerpPosition.x)),Mathf.FloorToInt(lerpPosition.y));

                //Limit pixel to actually be in scroll
                if (pixelPosition.x<border||pixelPosition.x>width/scale-border||pixelPosition.y<border||pixelPosition.y>height/scale-border)
                    continue;
                

                strokeColor.a=1;

                texture.SetPixel(pixelPosition.x,pixelPosition.y,strokeColor);

                strokeColor.a=.8f;
                if (Random.value<.75f && texture.GetPixel(pixelPosition.x,pixelPosition.y-1).a<strokeColor.a)
                                         texture.SetPixel(pixelPosition.x,pixelPosition.y-1,strokeColor);
                if (Random.value<.75f && texture.GetPixel(pixelPosition.x-1,pixelPosition.y).a<strokeColor.a)
                                         texture.SetPixel(pixelPosition.x-1,pixelPosition.y,strokeColor);
                if (Random.value<.75f && texture.GetPixel(pixelPosition.x,pixelPosition.y+1).a<strokeColor.a)
                                         texture.SetPixel(pixelPosition.x,pixelPosition.y+1,strokeColor);
                if (Random.value<.75f && texture.GetPixel(pixelPosition.x+1,pixelPosition.y).a<strokeColor.a)
                                         texture.SetPixel(pixelPosition.x+1,pixelPosition.y,strokeColor);
                if (Vector2.Distance(localMousePosition,lastMousePos)<Time.deltaTime*50){
                    if (Random.value<.75f && texture.GetPixel(pixelPosition.x,pixelPosition.y-2).a<strokeColor.a)
                                             texture.SetPixel(pixelPosition.x,pixelPosition.y-2,strokeColor);
                    if (Random.value<.75f && texture.GetPixel(pixelPosition.x-2,pixelPosition.y).a<strokeColor.a)
                                             texture.SetPixel(pixelPosition.x-2,pixelPosition.y,strokeColor);
                    if (Random.value<.75f && texture.GetPixel(pixelPosition.x,pixelPosition.y+2).a<strokeColor.a)
                                             texture.SetPixel(pixelPosition.x,pixelPosition.y+2,strokeColor);
                    if (Random.value<.75f && texture.GetPixel(pixelPosition.x+2,pixelPosition.y).a<strokeColor.a)
                                             texture.SetPixel(pixelPosition.x+2,pixelPosition.y,strokeColor);
                    if (Random.value<.75f && texture.GetPixel(pixelPosition.x-1,pixelPosition.y-1).a<strokeColor.a)
                                             texture.SetPixel(pixelPosition.x-1,pixelPosition.y-1,strokeColor);
                    if (Random.value<.75f && texture.GetPixel(pixelPosition.x-1,pixelPosition.y+1).a<strokeColor.a)
                                             texture.SetPixel(pixelPosition.x-1,pixelPosition.y+1,strokeColor);
                    if (Random.value<.75f && texture.GetPixel(pixelPosition.x+1,pixelPosition.y+1).a<strokeColor.a)
                                             texture.SetPixel(pixelPosition.x+1,pixelPosition.y+1,strokeColor);
                    if (Random.value<.75f && texture.GetPixel(pixelPosition.x+1,pixelPosition.y-1).a<strokeColor.a)
                                             texture.SetPixel(pixelPosition.x+1,pixelPosition.y-1,strokeColor);
                }
                if (Random.value<.25f && texture.GetPixel(pixelPosition.x,pixelPosition.y).a<strokeColor.a){
                    strokeColor.a=Random.Range(.25f,.75f);
                    pixelPosition+= Vector2Int.FloorToInt(Random.insideUnitCircle*3);
                    texture.SetPixel(pixelPosition.x,pixelPosition.y,strokeColor);
                }
            }


            texture.Apply();
        }
        lastMousePos=localMousePosition;
            
    }
    void EraseSpot(){
        Vector2Int localMousePosition = Vector2Int.FloorToInt(mousePos);

        List<Stroke> strokesToRemove = new List<Stroke>();
        foreach (Stroke s in strokes){
            if (s.texture.GetPixel(localMousePosition.x,localMousePosition.y).a!=0){
                strokesToRemove.Add(s);
                if(s.isStamp)
                    stamped=false;
            }
        }
        for (int i = strokesToRemove.Count-1; i >= 0;i--){
            DeleteStroke(strokesToRemove[i],true);
            lastGlyph=GlyphType.Invalid;
        } 
    }
    void DeleteStroke(Stroke stroke, bool changeSpell = false){
        stroke.Disappear();
        strokes.Remove(stroke);

        if (changeSpell){
            GlyphCategory cat = stroke.type.GetCategory();

            //If deleting a type that is part of the spell
            if (spell[cat]==stroke.type){

                spell[cat]=GlyphType.Invalid;//Remove it from the spell

                //Replace it with bonus if bonus exists
                if (spell.bonus.GetCategory()==cat)
                {
                    spell[cat]=spell.bonus;
                    spell.bonus=GlyphType.Invalid;
                }
            } 
            else if (spell.bonus==stroke.type){
                //If deleting bonus stroke
                spell.bonus=GlyphType.Invalid;
            }

            UpdateInkColor();
            
        }
    }

    void ActivateStroke(Stroke stroke){
        GlyphType type = stroke.type;
        GlyphCategory category = type.GetCategory();
        
        if (type == GlyphType.Invalid || spell[category] != GlyphType.Invalid){
            

            if (lastGlyph==type && type!=GlyphType.Invalid && spell.bonus==GlyphType.Invalid){
                //Allow any extra rune if repeated, but pretty much break the spell
                spell.bonus=type;
                UpdateInkColor();
                
            }else{
                //Otherwise remove invalid strokes
                if (tool!=Tool.Debug)
                    DeleteStroke(stroke);
                lastGlyph=type;

            }
            return;
        }
        
        spell[category]=type;
        lastGlyph=GlyphType.Invalid;
        UpdateInkColor();

    }


    void SpawnParticles(int amount){
        for (int i = 0 ; i < amount ; i++){
            float x = Random.Range(-transform.sizeDelta.x/2,transform.sizeDelta.x/2);
            float y = Random.Range(-transform.sizeDelta.y/2,transform.sizeDelta.y/2);


            PixController.CreatePix(transform.parent,new Vector2(x,y),2*scale,inkColor);
        }
    }

    void DrawBoxOnStroke(){
        //Dev tool: Grid
        GameObject go = new GameObject("Grid");
        RectTransform rt = go.AddComponent<RectTransform>();
        Image image = go.AddComponent<Image>();
        image.sprite=grid;
        rt.SetParent(activeStroke.transform);
        Vector2 gridPos = activeStroke.bounds.center*scale;
        gridPos-=new Vector2(width,height)/2;
        rt.anchoredPosition3D=gridPos;
        rt.sizeDelta=((Vector2Int)activeStroke.bounds.size)*scale;
        rt.localScale=Vector3.one;
        float stepSize = 1f/Glyph.size;
        for (int by = 0 ; by < Glyph.size; by+=1){
            for(int bx = 0 ; bx < Glyph.size; bx+=1){
                float tx = (bx/1f/Glyph.size);
                float ty = (by/1f/Glyph.size);
                BoundsInt boxBounds = new BoundsInt();
                
                Vector3Int boxMin = Vector3Int.FloorToInt(new Vector3(Mathf.Lerp(activeStroke.bounds.xMin,activeStroke.bounds.xMax,tx),         Mathf.Lerp(activeStroke.bounds.yMin,activeStroke.bounds.yMax,ty)));
                Vector3Int boxMax = Vector3Int.FloorToInt(new Vector3(Mathf.Lerp(activeStroke.bounds.xMin,activeStroke.bounds.xMax,tx+stepSize),Mathf.Lerp(activeStroke.bounds.yMin,activeStroke.bounds.yMax,ty+stepSize)));

                boxBounds.SetMinMax(boxMin,boxMax);
                if (activeStroke.glyph[bx,by] || (activeStroke.matchedGlyph!=null && activeStroke.matchedGlyph[bx,by])){
                    // Dev tool: squares
                    GameObject square = new GameObject($"Square {bx},{by} : {bx+by*Glyph.size}");
                    rt = square.AddComponent<RectTransform>();
                    image = square.AddComponent<Image>();
                    image.sprite=gridsquare;
                    image.color=new Color(1,0,1,.5f);

                    if (activeStroke.matchedGlyph!=null && !activeStroke.matchedGlyph[bx,by])                
                        image.color=new Color(1,0,0,.5f);
                    else if (!activeStroke.glyph[bx,by])
            
                        image.color=new Color(0,0,1,.5f);   

                    rt.SetParent(activeStroke.transform);
                    gridPos = boxBounds.center*scale;
                    gridPos-=new Vector2(width,height)/2;
                    rt.anchoredPosition3D=gridPos;
                    rt.sizeDelta=((Vector2Int)boxBounds.size)*scale;
                    rt.localScale=Vector3.one;
                }
            }
        }
    }

    void UpdateInkColor(){
        //Update color
        if (spell.bonus!=GlyphType.Invalid && !spell.bonus.IsElement()){
            inkColor=Color.black;
        }
        else if (spell.bonus!=GlyphType.Invalid && spell.bonus.IsElement()){
            inkColor=Color.Lerp(spell.element.GetElementColor(),spell.bonus.GetElementColor(),.5f);
        }
        else if (spell.element.IsElement()){
            inkColor=spell.element.GetElementColor();
            
        }
        else{
            inkColor=baseColor;
        }
        foreach(Stroke s in strokes){
            if (s.isActiveAndEnabled)
                s.ChangeColor(inkColor);
        }
    }

    void ControlDrag(){
        
        if (dragging){
            ((RectTransform)transform.parent).anchoredPosition = CanvasController.clampedCanvasMousePos + dragOffset;
            if (MyInput.clickUp)
                dragging=false;
        }
    }


    public void OnPointerDown(PointerEventData ped){
        GetClickInputs();
    }
    void GetClickInputs(){
        Vector2 scrollMousePos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform.parent,MyInput.mousePosition,Camera.main,out scrollMousePos);

        if (tool == Tool.None && ((RectTransform)transform.parent).rect.Contains(scrollMousePos)){
            if (MyInput.click)
            {
                dragOffset = ((RectTransform)transform.parent).anchoredPosition - CanvasController.clampedCanvasMousePos;
                dragging=true;
            }
        }
         //Limit mouse to actually be in scroll
        if (mousePos.x>=border && mousePos.x<=width/scale-border && mousePos.y>=border && mousePos.y<=height/scale-border)
        {
            if(tool == Tool.StampQuill){
                if (!isStampPad){
                    return;
                }
                if (MyInput.click)
                    CreateStroke();
            } 
            else if ((tool&Tool.Quill)!=Tool.None){
                if (MyInput.click){
                    CreateStroke();
                }    
            }
            else if (tool==Tool.Stamp && !stamped && StampPreviewController.good){
                if (MyInput.click){
                    CreateStamp();
                }
            }
            else if (tool == Tool.Eraser){
                if (MyInput.click){
                    erasing=true;
                }
            }
        }
    }
}
