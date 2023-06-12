using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
public class BookTabButton : MonoBehaviour,IPointerDownHandler
{
    public BookInfo.PageTabs tab;
    public int page;
    public bool onRight;
    public BookController bc;
    public Image image;
    public Sprite left,right;
    public GameObject leftTab,rightTab;
    public void Start(){
        bc=GetComponentInParent<BookController>();
        image=GetComponent<Image>();
        page = bc.bookInfo.PageOf(tab);
    }
    public void Update(){
        if (bc.pageNumber<=page)
        {
            image.sprite = right;
            leftTab.SetActive(false);
            rightTab.SetActive(true);
        }
        else{
            image.sprite = left;
            rightTab.SetActive(false);
            leftTab.SetActive(true);
        }
    }
    public void OnPointerDown(PointerEventData eventData)
    {
        if (eventData.button!=PointerEventData.InputButton.Left)
            return;
        bc.GoToPage(page);
    }
}
