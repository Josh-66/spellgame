using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PixController : MonoBehaviour{

    public RectTransform rt;

    float lifeTimer = 1;

    float rotationSpeed;
    public Vector2 velocity = Vector2.zero;
    void Awake(){
        rt=(RectTransform)transform;
        rt.anchoredPosition+=Random.insideUnitCircle*5;

        rotationSpeed=Random.Range(-180,180);
    }

    void Update(){
        lifeTimer-=Time.deltaTime/1;
        velocity=Vector2.Lerp(velocity,Vector2.zero,Time.deltaTime);
        rt.anchoredPosition+=velocity*Time.deltaTime;
        rt.localScale = Vector3.one*lifeTimer;
        rt.transform.rotation = Quaternion.Euler(0,0,rt.transform.rotation.eulerAngles.z+rotationSpeed*Time.deltaTime);
        if (lifeTimer<0)
            GameObject.Destroy(gameObject);
    }
    public static PixController CreatePix(Transform parent,Vector2 position,float size,Color color){
        GameObject g = Prefabs.Load("Pix");
        RectTransform rt = (RectTransform)g.transform;
        rt.SetParent(parent);
        rt.anchoredPosition3D=position;
        rt.sizeDelta=size*Vector2.one;
        rt.localScale=Vector3.one;
        g.GetComponent<Image>().color=color;

        return g.GetComponent<PixController>();
    }
}
