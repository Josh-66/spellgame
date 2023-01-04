using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolController : DeskObject
{
    public static Tool activeTool;//{get{return _activeTool.tool;}}

    private static ToolController _activeTool;
    public Tool tool;
    public Sprite active,inactive;
    public bool clicked = false;

    public override void Update(){
        base.Update();

        Sprite oldSprite = spriteRenderer.sprite;
        spriteRenderer.sprite = (int)(activeTool&tool)>0 ? active:inactive;
        if (oldSprite!=spriteRenderer.sprite){
            GameObject.Destroy(GetComponent<PolygonCollider2D>());
            gameObject.AddComponent<PolygonCollider2D>();
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
    public override void Activate(){
        Debug.Log(_activeTool);
        if (_activeTool=this){
            _activeTool=null;
            activeTool=Tool.None;
            return;
        }
        if (_activeTool!=null){
            _activeTool.Deactivate();
        }
        _activeTool=this;
        activeTool=this.tool;
        Debug.Log("here2"+activeTool);
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