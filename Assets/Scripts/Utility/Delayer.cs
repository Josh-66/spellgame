using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class Delayer : MonoBehaviour
{
    static Delayer instance{
        get{
            if (_instance==null)
                _instance=(new GameObject("Delayer")).AddComponent<Delayer>();
            return _instance;
        }
    }
    static Delayer _instance;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    IEnumerator DelayActionCR((float,Action) order){
        for (float i = 0 ; i < order.Item1;i+=Time.deltaTime){
            yield return null;
        }
        order.Item2();
    }

    public static void DelayAction(float time, Action action){
        instance.StartCoroutine("DelayActionCR",(time,action));
    }
}
