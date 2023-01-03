using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
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

    // Start is called before the first frame update
    void Start()
    {
        Glyph.LoadGlyphs();
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHovered();
        UpdateClicked();
    }


    void UpdateHovered(){
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject()){
            hoveredObject=null;
        }
        else{
            RaycastHit2D hit = Physics2D.Raycast(point,Vector2.up,.01f,LayerMask.GetMask("Clickable"));
            if (hit){
                hoveredObject=hit.collider.GetComponent<Clickable>();
            }
            else{
                hoveredObject=null;
            }
        }
    }
    void UpdateClicked(){
        if (hoveredObject!=null && MyInput.click)
            hoveredObject.OnClick();
    }
}
