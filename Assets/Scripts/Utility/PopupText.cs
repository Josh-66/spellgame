using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopupText : MonoBehaviour
{

    public float speed = 10;
    public float lifeTimer=1.5f;
    public TMPro.TextMeshProUGUI tm;
    new RectTransform transform {get{return (RectTransform)base.transform;}}

    Vector2 startingPos,targetPos;
    // Start is called before the first frame update
    void Start()
    {
        startingPos=transform.anchoredPosition;
        targetPos=startingPos+Vector2.up*50;
    }

    // Update is called once per frame
    void Update()
    {
        transform.anchoredPosition= Vector2.Lerp(transform.anchoredPosition,targetPos,speed*Time.deltaTime);

        Color c = tm.color;
        c.a=lifeTimer;
        tm.color=c;
        lifeTimer-=Time.deltaTime;
        if (lifeTimer<0)
            GameObject.Destroy(gameObject);
    }

    public static PopupText Create(Color color, RectTransform originalParent, Vector3 anchoredPosition,string text){
        GameObject popup = Prefabs.Load("PopupText");
        PopupText pt = popup.GetComponent<PopupText>();
        
        pt.tm =popup.GetComponent<TMPro.TextMeshProUGUI>();
        pt.tm.raycastTarget=false;
        pt.tm.text=text;
        pt.tm.color=color;
        pt.transform.SetParent(originalParent,false);
        ((RectTransform)pt.transform).anchoredPosition=anchoredPosition;
        pt.transform.localScale=Vector3.one;
        pt.transform.SetParent(CanvasController.textLayer,true);

        return pt;

        
    }
}
