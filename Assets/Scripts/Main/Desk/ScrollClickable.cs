using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollClickable : DeskObject
{
    public static ScrollClickable instance;
    
    public Material scrollInkMaterial;
    public Sprite open,rolled;
    public int baseSortingOrder;
    public bool beingGiven;
    AudioSource audioSource;
    public AudioClip rolledSound,unrolledSound;
    public override void Awake(){
        base.Awake();
        instance=this;
        baseSortingOrder=spriteRenderer.sortingOrder;
        audioSource=GetComponent<AudioSource>();
    }
    public override void Update(){
        base.Update();
        spriteRenderer.material=(hovered|held|clickTimer>=0|beingGiven) ? regular:outlined; 
        
        if (transform.localPosition.y>4.5f && held && ScrollController.instance.inkController.stamped && GameController.submittable){
            if (spriteRenderer.sprite!=rolled){
                ScrollController.CloseScroll();
                spriteRenderer.sprite=rolled;
                
                audioSource.clip=rolledSound;
                audioSource.Play();

                foreach(Transform child in transform){
                    child.gameObject.SetActive(false);
                }
                GameObject.Destroy(GetComponent<PolygonCollider2D>());
                gameObject.AddComponent<PolygonCollider2D>();
            }
        }
        else if (transform.localPosition.y<1.5f && !beingGiven){
            if (spriteRenderer.sprite!=open){
                spriteRenderer.sprite=open;
                foreach(Transform child in transform){
                    child.gameObject.SetActive(true);
                }
                
                audioSource.clip=unrolledSound;
                audioSource.Play();

                GameObject.Destroy(GetComponent<PolygonCollider2D>());
                gameObject.AddComponent<PolygonCollider2D>();
            }
        }
        if (spriteRenderer.sprite==rolled && MyInput.clickUp){
            foreach(Collider2D col in GetComponents<Collider2D>()){
                col.enabled=false;
            }
            rigidbody2D.velocity=rigidbody2D.velocity.normalized*.5f;
            spriteRenderer.sortingOrder=-1;
            beingGiven=true;
            GameController.instance.SubmitSpell(ScrollController.instance.inkController.spell);
            ScrollController.instance.inkController.HardClearStrokes();
        }
        if (transform.position.y<-5){
            beingGiven=false;
            foreach(Collider2D col in GetComponents<Collider2D>()){
                col.enabled=true;
            }
            spriteRenderer.sortingOrder=baseSortingOrder;
            CustomerController.instance.ExitScrollDropped();

            Respawn();
        }
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
        mr.material=scrollInkMaterial;
        mr.material.mainTexture=s.texture;
        mr.sortingLayerID=spriteRenderer.sortingLayerID;
        mr.sortingOrder=spriteRenderer.sortingOrder+1;
        
        stroke.transform.localPosition=Vector3.zero;
        stroke.transform.localRotation=Quaternion.identity;

        return stroke;
    }
    public override void Activate()
    {
        if (spriteRenderer.sprite==open){
            ScrollController.ToggleScroll();
        }
    }
    public override void Respawn()
    {
        base.Respawn();
        
    }
}
