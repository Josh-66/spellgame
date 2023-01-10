using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public class BookController : MonoBehaviour, IPointerDownHandler
{
    public static BookController instance;
    public static bool isOpen {get{return instance.gameObject.activeSelf;}}
    public bool dragging = false;
    public Vector2 dragOffset;
    
    public AudioSource source;
    public AudioClip pickUp,putDown;
    public AudioSource pageSource;
    public AudioClip pageTurn;

    public int pageNumber{
        get{
            return _pageNumber;
        }
        set{
            _pageNumber=value;
            LoadPage(_pageNumber);
        }
    }
    int _pageNumber;
    public BookInfo bookInfo;

    public MonoBehaviour activePageController{
        set{
            if(_activePageController!=null)
                _activePageController.gameObject.SetActive(false);
            _activePageController=value;
            _activePageController.gameObject.SetActive(true);
        }
    }
    MonoBehaviour _activePageController;
    public QuickRefBookController quickRefBookController;
    public IndexPageBookController indexPageBookController;
    public static void OpenBook(){
        instance.gameObject.SetActive(true);
        instance.dragging=false;
        
        instance.source.clip=instance.pickUp;
        instance.source.Play();
    }
    public static void CloseBook(bool silent = false){
        instance.gameObject.SetActive(false);
        instance.source.clip=instance.putDown;
        if (!silent)
            instance.source.Play();
    }
    public static void ToggleBook(){
        instance.gameObject.SetActive(!isOpen);
        instance.source.clip=isOpen ? instance.pickUp : instance.putDown;
        instance.source.Play();

    }
    void Awake(){
        
        instance=this;
        pageNumber=0;
        CloseBook(true);
    }
    void Update() {
        ControlDrag();

        
    }
    void LoadPage(int number){
        bookInfo.pages[number].Activate(this);
        pageSource.clip=pageTurn;
        pageSource.Play();
    }
    public void CloseButton(){
        if (!MyInput.click)
            return;
        CloseBook();
    }
    public void PageLeft(){
        if (!MyInput.click)
            return;
        if (pageNumber>0)
            pageNumber--;
    }
    public void PageRight(){
        if (!MyInput.click)
            return;
        if (pageNumber<bookInfo.pages.Count-1){
            pageNumber++;
        }
    }
    public void ElementTab(){
        if (!MyInput.click)
            return;
        pageNumber=0;
    }
    public void FormTab(){
        if (!MyInput.click)
            return;
        pageNumber=2;
    }
    public void StrengthTab(){
        if (!MyInput.click)
            return;
        pageNumber=4;
    }
    public void StyleTab(){
        if (!MyInput.click)
            return;
        pageNumber=5;
    }
    public void RulesTab(){
        if (!MyInput.click)
            return;
    }
    public void IndexTab(){
        if (!MyInput.click)
            return;
    }


    public void OnPointerDown(PointerEventData ped){
        // Vector2 scrollMousePos;
        // RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform,Input.mousePosition,Camera.main,out scrollMousePos);

        // if (((RectTransform)transform).rect.Contains(scrollMousePos)){
            if (MyInput.click)
            {
                dragOffset = ((RectTransform)transform).anchoredPosition - CanvasController.clampedCanvasMousePos;
                dragging=true;
            }
        // }
    }
    void ControlDrag(){
        
        if (dragging){
            ((RectTransform)transform).anchoredPosition = CanvasController.clampedCanvasMousePos + dragOffset;
            if (MyInput.clickUp)
                dragging=false;
        }
    }


}
