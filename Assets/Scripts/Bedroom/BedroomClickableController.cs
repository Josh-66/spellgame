using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedroomClickableController : MonoBehaviour
{
    public Clickable hoveredObject{get{
        return _hoveredObject;
    }set{
        if (_hoveredObject!=null)
            _hoveredObject.OffHover();
        _hoveredObject=value;
        if (_hoveredObject!=null){
            value.OnHover();
        }
    }}
    public Clickable _hoveredObject;

    public void Update(){
        UpdateClicked();
        UpdateHovered();
    }
    void UpdateHovered(){
        Vector3 point = MyInput.WorldMousePos();

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject() || MyInput.clickHeld){
            hoveredObject=null;
        }
        else{
            RaycastHit2D hit = Physics2D.Raycast(point,Vector2.up,.01f,LayerMask.GetMask("Clickable"));
            if (hit){
                hoveredObject=hit.collider.GetComponent<Clickable>();
            }
            else{
                RaycastHit hit2;
                bool hit3 = Physics.Raycast(MyInput.WorldMouseRay(),out hit2, 20f,LayerMask.GetMask("Clickable"),QueryTriggerInteraction.Collide);
                if (hit3)
                {
                    hoveredObject=hit2.collider.GetComponent<Clickable>();
                }
                else{
                    hoveredObject=null;
                }
            }
        }
    }
    void UpdateClicked(){
        if (hoveredObject!=null && MyInput.click)
        {
            hoveredObject.OnClick();
        }
    }
}
