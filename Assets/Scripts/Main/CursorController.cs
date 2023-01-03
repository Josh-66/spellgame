using UnityEngine;
using UnityEngine.UI;

public class CursorController : MonoBehaviour{
    
    public Sprite eraser,stamp;
    public Sprite quill1,quill2,quill3;
    public Sprite quill {get => ScrollController.instance.inkController.brushTilt switch{
        <10f => quill1,
        <30f => quill2,
        _ => quill3,
    };}

    public Image image;
    new RectTransform transform{get{return (RectTransform)base.transform;}}
    public void Start(){
        image = GetComponent<Image>();
    }
    void Update(){
        if (ScrollController.isOpen){
            
            
            image.sprite = ToolController.activeTool switch{
                Tool.Quill=>quill,
                Tool.Eraser=>eraser,
                Tool.Stamp=>stamp,
                _=>null,
            };

            if (image.sprite==null){
                Cursor.visible=true;
                image.enabled=false;
            }
            else{
                transform.pivot=new Vector2(image.sprite.pivot.x/image.sprite.textureRect.width,image.sprite.pivot.y/image.sprite.textureRect.height);
                Vector2 mousePosition;
                RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasController.transform,Input.mousePosition,Camera.main,out mousePosition);
                transform.anchoredPosition=mousePosition;

                Debug.Log(CanvasController.transform.rect);

                if (mousePosition.x < CanvasController.transform.rect.xMin || mousePosition.y < CanvasController.transform.rect.yMin || mousePosition.x > CanvasController.transform.rect.xMax || mousePosition.y > CanvasController.transform.rect.yMax)
                    Cursor.visible=true;
                else
                    Cursor.visible=false;
                image.SetNativeSize();
                image.enabled=true;
            }
        
        }
        else{
            Cursor.visible=true;
            image.enabled=false;
        }
    }
}