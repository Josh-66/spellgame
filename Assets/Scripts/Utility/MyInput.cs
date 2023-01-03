using UnityEngine;
public class MyInput : MonoBehaviour{
    static MyInput instance{get{
        if (_instance==null)
            _instance = (new GameObject("Input")).AddComponent<MyInput>();
        return _instance;
    }}
    static MyInput _instance;

    public static bool click{
        get{
            if (instance._click)
            {
                instance._click=false;
                return true;
            }
            else return false;
        }
        set{instance._click=value;}
    }
    bool _click;
    public static bool clickUp;
    public static bool clickHeld;
    void Update(){

        _click = (Input.GetMouseButtonDown(0));
        clickUp=Input.GetMouseButtonUp(0);
    }
}