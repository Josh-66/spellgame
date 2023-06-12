using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class TitleButtonController : MonoBehaviour,IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public Button.ButtonClickedEvent onClick;
    public bool hovered;
    public RectTransform child;

    void Update() {
        float targPosition = hovered ? 150:0;
        child.anchoredPosition = Vector2.Lerp(child.anchoredPosition,Vector2.right*targPosition,Time.deltaTime*15f);
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button!=PointerEventData.InputButton.Left)
            return;
        onClick.Invoke();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hovered=true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hovered=false;
    }
}
