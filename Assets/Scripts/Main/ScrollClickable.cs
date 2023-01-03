using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollClickable : MonoBehaviour, Clickable
{
    public static ScrollClickable instance;
    public SpriteRenderer spriteRenderer;
    public Material outlined,regular;

    
    new public Rigidbody2D rigidbody2D;
    public Vector3 basePosition;
    
    float clickTimer=-1f;
    Vector3 clickMousePosition;
    public bool held = false;
    bool hovered;
    public void Awake(){
        instance=this;
        spriteRenderer=GetComponent<SpriteRenderer>();
        rigidbody2D=GetComponent<Rigidbody2D>();
        basePosition=transform.position;
    }

    void Update() {
       
        spriteRenderer.material=hovered ? regular:outlined; 

        if (clickTimer>=0){
            clickTimer-=Time.deltaTime;
            if (clickTimer<0)
                held=true;
            if ((Input.mousePosition-clickMousePosition).magnitude>30f){
                clickTimer=-1;
                held=true;
            }
        }
        if (MyInput.clickUp){
            if (clickTimer>0){
                ScrollController.ToggleScroll();
            }
            held=false;
            clickTimer=-1f;
        }       
    }
    void FixedUpdate() {
        if (held){
            
            rigidbody2D.velocity = (Camera.main.ScreenToWorldPoint(Input.mousePosition)-transform.position) * 10f;
            rigidbody2D.angularVelocity = -transform.rotation.eulerAngles.z;
            if (rigidbody2D.angularVelocity <-180f)
                rigidbody2D.angularVelocity+=360f;
        }
        if (transform.position.y<-50){
            rigidbody2D.velocity=Vector2.zero;
            rigidbody2D.angularVelocity=0;
            transform.rotation=Quaternion.Euler(0,0,0);
            transform.position=basePosition;
            PoofController.Create(transform.position,2);
        }
    }
    public void OffHover()
    {
        hovered=false;
    }

    public void OnClick()
    {
        clickTimer=.5f;
        clickMousePosition=Input.mousePosition;
    }
    public void OnHover()
    {
        hovered=true;
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
}
