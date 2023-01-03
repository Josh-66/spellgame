using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class DownButton : MonoBehaviour,IPointerDownHandler
{
    public Button.ButtonClickedEvent onClick;

    public void OnPointerDown(PointerEventData eventData)
    {
        onClick.Invoke();
    }
}
