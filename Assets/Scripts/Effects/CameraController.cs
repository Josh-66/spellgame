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
    Animator animator;
    public bool shake=true;
    public bool arrived = false;
    void Awake(){
        instance=this;
    }
    void Start()
    {
        animator=GetComponent<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (arrived && shake){
            velocity = Vector3.Lerp(velocity,targPosition+targShake-transform.position,Time.deltaTime*.35f);
            transform.position+=Time.deltaTime*velocity;
            if (Vector3.Distance(transform.position-targPosition,targShake)<.1f){
                targPosition=Random.insideUnitCircle*.2f;
                targPosition.z=-10;
            }
            transform.rotation=Quaternion.Euler(0,0,Mathf.Sin(t)/2);
            t+=Time.deltaTime*.2f;
            t %=2 * Mathf.PI;
        }



        
    }
    public void Arrive(){
        arrived=true;
        animator.enabled=false;
        targPosition=transform.position;

    }
    public void UnArrive(){
        arrived=false;

    }
}