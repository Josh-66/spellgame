using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PixController : MonoBehaviour{

    public RectTransform rt;

    float lifeTimer = 1;

    float rotationSpeed;
    void Awake(){
        rt=(RectTransform)transform;
        rt.anchoredPosition+=Random.insideUnitCircle*5;

        rotationSpeed=Random.Range(-180,180);
    }

    void Update(){
        lifeTimer-=Time.deltaTime/1;

        rt.localScale = Vector3.one*lifeTimer;
        rt.transform.rotation = Quaternion.Euler(0,0,rt.transform.rotation.eulerAngles.z+rotationSpeed*Time.deltaTime);
        if (lifeTimer<0)
            GameObject.Destroy(gameObject);
    }
}
