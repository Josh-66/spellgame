using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class DaylightController : MonoBehaviour
{    public float targetTime;
    public float time;
    public Light2D globalLight;
    public Gradient gradient;
    public Color currentColor => gradient.Evaluate(time);
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        time=Mathf.MoveTowards(time,targetTime,Time.deltaTime/50f);
        globalLight.color=currentColor;
    }
}
