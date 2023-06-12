using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolController : DeskObject
{
    public static Tool activeTool;//{get{return _activeTool.tool;}}

    private static ToolController activeToolController;
    public Tool tool;
    public Sprite active,inactive;
    public bool clicked = false;
    public AudioClip grab,place;
    AudioSource audioSource;
    public override void Awake()
    {
        base.Awake();
        audioSource=GetComponent<AudioSource>();
    }

    public override void Update(){
        base.Update();

        // Sprite oldSprite = spriteRenderer.sprite;
        spriteRenderer.sprite = (int)(activeTool&tool)>0 ? active:inactive;
        // if (oldSprite!=spriteRenderer.sprite){
        //     GameObject.Destroy(GetComponent<PolygonCollider2D>());
        //     gameObject.AddComponent<PolygonCollider2D>();
        // }
    }

    public static void ClearTool(){
        if (activeToolController!=null){
            activeToolController.Deactivate();
        }
    }
    public void Deactivate(){
        if (activeToolController=this){
            activeToolController=null;
            activeTool=Tool.None;
            audioSource.clip=place;
            audioSource.Play();
        }
    }
    public override void Activate(){
        if (activeToolController==this){
            Deactivate();
            return;
        }
        if (activeToolController!=null){
            activeToolController.Deactivate();
        }
        
        audioSource.clip=grab;
        audioSource.Play();
        activeToolController=this;
        activeTool=this.tool;
    }
}

public enum Tool{
    None = 0,
    Quill      = 0b00001,
    Debug      = 0b01001,
    StampQuill = 0b10001,
    Eraser     = 0b00010,
    Stamp      = 0b00100,
}