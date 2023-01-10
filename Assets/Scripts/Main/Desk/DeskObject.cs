using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeskObject : MonoBehaviour, Clickable
{
    public bool hovered;
    [HideInInspector] public SpriteRenderer spriteRenderer;
    public static Material outlined,regular;

    
    [HideInInspector] new public Rigidbody2D rigidbody2D;
    [HideInInspector] public Vector3 basePosition;

    [HideInInspector] public float clickTimer=-1f;
    [HideInInspector] public Vector3 clickMousePosition;
    public bool held = false;
    public virtual void Awake()
    {
        spriteRenderer=GetComponent<SpriteRenderer>();
        rigidbody2D=GetComponent<Rigidbody2D>();
        basePosition=transform.position;
        spriteRenderer.material=outlined;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        spriteRenderer.material=hovered ? regular:outlined; 

        if (clickTimer>=0){
            clickTimer-=Time.deltaTime;
            if (clickTimer<0)
                held=true;
            if ((Input.mousePosition-clickMousePosition).magnitude>10f){
                clickTimer=-1;
                held=true;
            }
        }
        if (MyInput.clickUp){
            if (clickTimer>0){
                Activate();
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
    public virtual void Activate(){

    }
}
