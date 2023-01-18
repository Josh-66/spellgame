using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.EventSystems;

public abstract class WindowController : MonoBehaviour, IPointerDownHandler
{

    public bool dragging = false;
    public Vector2 dragOffset;
    public AudioSource source;
    public AudioClip pickUp,putDown;


    public virtual void OnPointerDown(PointerEventData ped){
        // Vector2 scrollMousePos;
        // RectTransformUtility.ScreenPointToLocalPointInRectangle((RectTransform)transform,MyInput.mousePosition,Camera.main,out scrollMousePos);

        // if (((RectTransform)transform).rect.Contains(scrollMousePos)){
            if (MyInput.click)
            {
                dragOffset = ((RectTransform)transform).anchoredPosition - CanvasController.clampedCanvasMousePos;
                dragging=true;
            }
        // }
    }
    public virtual void Update() {
        ControlDrag();
    }
    void ControlDrag(){
        
        if (dragging){
            ((RectTransform)transform).anchoredPosition = CanvasController.clampedCanvasMousePos + dragOffset;
            if (MyInput.clickUp)
                dragging=false;
        }
    }
    public void Open(){
        gameObject.SetActive(true);
        dragging=false;
        
        source.clip=pickUp;
        source.Play();
    }
    public void Close(bool silent = false){
        if (!gameObject.activeSelf)
            return;
        gameObject.SetActive(false);
        source.clip=putDown;
        if (!silent)
            source.Play();
    }
    public void Toggle(){
        if (gameObject.activeSelf){
            Close();
        }
        else{
            Open();
        }
    }
    public abstract void Activate();

    
}