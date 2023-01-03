using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookClickable : MonoBehaviour,Clickable
{
    public bool hovered;
    public SpriteRenderer spriteRenderer;
    public Material outlined,regular;

    
    new public Rigidbody2D rigidbody2D;
    public Vector3 basePosition;
    void Awake(){
        spriteRenderer=GetComponent<SpriteRenderer>();
        rigidbody2D=GetComponent<Rigidbody2D>();
        basePosition=transform.position;
    }
    void Update() {
       
        spriteRenderer.material=hovered ? regular:outlined;
        
    }
    void FixedUpdate() {
        if (transform.position.y<-50){
            rigidbody2D.velocity=Vector2.zero;
            rigidbody2D.angularVelocity=0;
            transform.rotation=Quaternion.Euler(0,0,0);
            transform.position=basePosition;
            PoofController.Create(transform.position,1.5f);
        }
    }

    public void OffHover()
    {
        hovered=false;
    }

    public void OnClick()
    {
        BookController.ToggleBook();
    }

    public void OnHover()
    {
        hovered=true;
    }

}
