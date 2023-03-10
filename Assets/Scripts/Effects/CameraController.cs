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
    void Awake(){
        instance=this;
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (shake)
            velocity = Vector3.Lerp(velocity,targPosition+targShake-transform.localPosition,Time.deltaTime*.35f);
            transform.localPosition+=Time.deltaTime*velocity;
            if (Vector3.Distance(transform.localPosition-targPosition,targShake)<.1f){
                targPosition=Random.insideUnitCircle*.2f;
                targPosition.z=0;
        }
        if (rotate){
            transform.rotation=Quaternion.Euler(0,0,Mathf.Sin(t)/2);
            t+=Time.deltaTime*.2f;
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
