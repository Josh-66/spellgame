using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpObjectController : MonoBehaviour
{
    public Transform target;
    Vector2 offset;
    Vector2 targetScreenPoint{
        get{
            Vector2 localPoint = RectTransformUtility.WorldToScreenPoint(Camera.main,target.position);
            RectTransformUtility.ScreenPointToLocalPointInRectangle(((RectTransform)rectTransform.parent.transform),localPoint,Camera.main,out localPoint);         
            return localPoint;
        }
    }
    RectTransform rectTransform;
    public void InitializePosition()
    {
        rectTransform=transform as RectTransform;

        // offset=rectTransform.anchoredPosition-targetScreenPoint;
    }

    public void SetPosition() {
        if (target.gameObject.activeSelf)
            rectTransform.anchoredPosition=targetScreenPoint;        
        else
            rectTransform.anchoredPosition=Vector2.negativeInfinity;
    }
    void Update() {
        SetPosition();
    }
}
