using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollClickable : DeskObject
{
    public static ScrollClickable instance;
    
    
    public override void Awake(){
        base.Awake();
        instance=this;
        
    }

    public GameObject AddStroke(Sprite s){
        GameObject stroke = new GameObject("Stroke");
        stroke.transform.parent=transform;

        // SpriteRenderer sr = stroke.AddComponent<SpriteRenderer>();
        // sr.sprite=s;
        // sr.material=ScrollController.instance.inkController.inkMaterial;

        Mesh m = new Mesh();
        m.vertices = new Vector3[]{new Vector3(-2,-1.3f,0),new Vector3(2,-1.3f,0),new Vector3(1.4f,1f,0),new Vector3(-1.3f,1f,0)};
        m.triangles = new int[]{0,1,2,2,3,0};
        m.uv = new Vector2[]{Vector3.zero,Vector3.right,new Vector3(1,1,0),Vector3.up};

        MeshRenderer mr = stroke.AddComponent<MeshRenderer>();
        MeshFilter mf = stroke.AddComponent<MeshFilter>();
        mf.mesh=m;
        mr.material=ScrollController.instance.inkController.inkMaterial;
        mr.material.mainTexture=s.texture;
        mr.sortingLayerID=spriteRenderer.sortingLayerID;
        mr.sortingOrder=spriteRenderer.sortingOrder;
        
        stroke.transform.localPosition=Vector3.back*.01f;


        return stroke;
    }
    public override void Activate()
    {
        ScrollController.ToggleScroll();
    }
}
