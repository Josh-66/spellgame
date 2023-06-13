using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController instance;
    public Vector3 targShake;
    public Vector3 targPosition;
    public Vector3 velocity;
    public float t;
    public Animator animator;
    public bool shake=true;
    public bool rotate= true;
    public bool arrived {get{return transform.position.x>-1;}}
    public Vector3 lastPosition;
    //public float highestDT,highestUDT;
    void Awake(){
        instance=this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //highestDT=Mathf.Max(highestDT,Time.deltaTime);
        //highestUDT=Mathf.Max(highestUDT,Time.unscaledDeltaTime);
        
        if (shake)
            velocity = Vector3.Lerp(velocity,targPosition+targShake-transform.localPosition,Time.deltaTime/Time.timeScale*.35f);
            transform.localPosition+=Time.deltaTime/Time.timeScale*velocity;
            if (Vector3.Distance(transform.localPosition-targPosition,targShake)<.1f){
                targPosition=Random.insideUnitCircle*.2f;
                targPosition.z=0;
            if (Vector3.Distance(transform.localPosition,targPosition)>1f){
                transform.localPosition= (targPosition) + (transform.localPosition-targPosition).normalized;
            }
        }
        if (rotate){
            transform.rotation=Quaternion.Euler(0,0,Mathf.Sin(t)/2);
            t+=Time.deltaTime/Time.timeScale*.2f;
            t %=2 * Mathf.PI;
        }
    }
    public void Arrive(){
        targPosition=transform.localPosition;

    }
    public void UnArrive(){
        animator.SetTrigger("Exit");

    }
}
