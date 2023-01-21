using UnityEngine;
public class MyInput : MonoBehaviour{
    // static MyInput instance{get{
    //     if (_instance==null)
    //         _instance = (new GameObject("Input")).AddComponent<MyInput>();
    //     return _instance;
    // }}
    // static MyInput _instance;
    public static Vector3 mousePosition{get{
        return Input.mousePosition;
    }}

    public static bool click{
        get{
            return Input.GetMouseButtonDown(0);
            // if (instance._click)
            // {
            //     instance._click=false;
            //     return true;
            // }
            // else return false;
        }
        // set{instance._click=value;}
    }
    // bool _click;
    public static bool clickUp{get{return Input.GetMouseButtonUp(0);}}
    public static bool clickHeld{get{return Input.GetMouseButton(0);}}
    // void Update(){

    //     _click = (Input.GetMouseButtonDown(0));
    //     clickUp=Input.GetMouseButtonUp(0);
    //     clickHeld=Input.GetMouseButton(0);
    // }
    public static Vector3 WorldMousePos(float z = 0){
        
        Ray ray=WorldMouseRay();

        Plane xy = new Plane(Vector3.forward, new Vector3(0, 0, z));
        float distance;
        xy.Raycast(ray, out distance);
        return new Vector3(ray.GetPoint(distance).x,ray.GetPoint(distance).y,z);
    }
    public static Ray WorldMouseRay(){
        
        Ray ray;
        ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        return ray;
    }
}