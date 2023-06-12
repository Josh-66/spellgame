using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DownButton : MonoBehaviour,IPointerDownHandler,IPointerUpHandler
{
    public Button.ButtonClickedEvent onClick;
    public Button.ButtonClickedEvent onClickUp;
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button!=PointerEventData.InputButton.Left)
            return;
        onClick.Invoke();
    }
    public void OnPointerUp(PointerEventData eventData)
    {
        if (eventData.button!=PointerEventData.InputButton.Left)
            return;
        onClickUp.Invoke();
    }
}
