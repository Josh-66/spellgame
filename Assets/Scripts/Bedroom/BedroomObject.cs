using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomObject : MonoBehaviour,Clickable
{
    public static Material outlined,regular;
    public SpriteRenderer spriteRenderer;
    public bool hovered;
    public virtual void Awake() {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.material=regular;
    }
    public virtual void Update(){
        spriteRenderer.material =  hovered ? regular:outlined;
    }
    public void OffHover()
    {
        hovered=false;
    }

    public void OnClick()
    {
        Activate();
    }

    public void OnHover()
    {
        hovered = true;
    }

    public virtual void Activate(){

    }
}
