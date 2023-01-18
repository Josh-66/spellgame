using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class StampPreviewController : MonoBehaviour
{
    public static StampPreviewController instance;
    public Image image;
    public RectTransform parent;
    new public RectTransform transform;
    public static bool good = false;
    void Awake(){
        transform=(RectTransform)base.transform;
        parent=(RectTransform)transform.parent.transform;
        instance=this;
        image=GetComponent<Image>();
    }
    public void Update(){
        if (ToolController.activeTool==Tool.Stamp ){
            image.enabled=true;
            Vector2 mousePos = MyInput.mousePosition;
            //localMousePosition = Camera.main.ScreenToWorldPoint(localMousePosition);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(parent,mousePos,Camera.main,out mousePos);
            transform.anchoredPosition=Vector2Int.FloorToInt(mousePos/InkController.scale)*InkController.scale;
            transform.SetAsLastSibling();

            Vector2 max = transform.anchoredPosition+transform.sizeDelta/2;
            Vector2 min = transform.anchoredPosition-transform.sizeDelta/2;

            good = false;
            if (Vector2.Max(max,parent.sizeDelta/2)==parent.sizeDelta/2 && Vector2.Min(min,-parent.sizeDelta/2)==-parent.sizeDelta/2){
                good=true;
            }
            image.color = good ? new Color(0,0,0,.5f) : new Color(1,.25f,.25f,.5f);
        }
        else{
            good = false;
            image.enabled=false;
        }
    }
}