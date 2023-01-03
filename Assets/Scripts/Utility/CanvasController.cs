using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CanvasController : MonoBehaviour
{
    public static Canvas canvas;
    public static UnityEngine.UI.CanvasScaler scaler;
    new public static RectTransform transform;
    public static RectTransform textLayer;
    public static Vector2 canvasMousePos{get{
        Vector2 mousePos = Input.mousePosition;
        //localMousePosition = Camera.main.ScreenToWorldPoint(localMousePosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasController.transform,mousePos,Camera.main,out mousePos);
        return mousePos;
        }
    }
    public static Vector2 clampedCanvasMousePos{get{
        Vector2 mousePos = Input.mousePosition;
        //localMousePosition = Camera.main.ScreenToWorldPoint(localMousePosition);
        RectTransformUtility.ScreenPointToLocalPointInRectangle(CanvasController.transform,mousePos,Camera.main,out mousePos);
        mousePos.x = Mathf.Max(mousePos.x,transform.rect.xMin+40); 
        mousePos.x = Mathf.Min(mousePos.x,transform.rect.xMax-40);
        mousePos.y = Mathf.Max(mousePos.y,transform.rect.yMin+40);
        mousePos.y = Mathf.Min(mousePos.y,transform.rect.yMax-40);
        return mousePos;
        }
    }
    void Awake(){
        canvas=GetComponent<Canvas>();
        scaler=GetComponent<UnityEngine.UI.CanvasScaler>();
        transform=GetComponent<RectTransform>();
        textLayer=(RectTransform)transform.Find("TextLayer");
    }

}
