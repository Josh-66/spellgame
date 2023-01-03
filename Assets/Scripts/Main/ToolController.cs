using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolController : MonoBehaviour,Clickable
{
    public static Tool activeTool;//{get{return _activeTool.tool;}}

    private static ToolController _activeTool;
    public Tool tool;

    public SpriteRenderer spriteRenderer;
    new public Rigidbody2D rigidbody2D;
    public Vector3 basePosition;
    public Sprite active,inactive;
    public Material outlined,regular;
    public bool hovered = false;
    public bool held;
    public bool clicked = false;
    public void Awake(){
        spriteRenderer=GetComponent<SpriteRenderer>();
        spriteRenderer.material=outlined;

        rigidbody2D=GetComponent<Rigidbody2D>();
        basePosition=transform.position;
    }

    public void Update(){
        Sprite oldSprite = spriteRenderer.sprite;
        spriteRenderer.sprite= _activeTool==this ? active:inactive;
        if (oldSprite!=spriteRenderer.sprite){
            GameObject.Destroy(GetComponent<PolygonCollider2D>());
            gameObject.AddComponent<PolygonCollider2D>();
        }

        if (!hovered && !held){
            spriteRenderer.material=outlined;
        }
        if (hovered || held){
            spriteRenderer.material=regular;
        }

        
        if (MyInput.clickUp)
            held=false;
      
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
            PoofController.Create(transform.position,1);
        }
    }
    public static void ClearTool(){
        if (_activeTool!=null){
            _activeTool.Deactivate();
        }
    }
    public void Deactivate(){
        if (_activeTool=this){
            _activeTool=null;
            activeTool=Tool.None;
        }
    }
    public void Activate(){
        if (_activeTool!=null){
            _activeTool.Deactivate();
        }
        _activeTool=this;
        activeTool=this.tool;
    }

    public void OnHover() {
        hovered=true;
    }
    public void OffHover() {
        hovered=false;
    }
    public void OnClick(){
        if (ScrollController.isOpen){
            if (_activeTool==this)
            {
                Deactivate();
            }
            else
                Activate();
        }
        else{
            held=true;
        }
    }
}

public enum Tool{
    None,
    Quill,
    StampQuill,
    Eraser,
    Stamp,
    Debug,
}