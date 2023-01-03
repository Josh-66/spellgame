using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoofController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.childCount==0)
            GameObject.Destroy(gameObject);
    }

    public static void Create(Vector3 worldPos,float scale){
        GameObject poof = GameObject.Instantiate<GameObject>(Resources.Load<GameObject>("Poof"));
        poof.transform.position=worldPos;
        poof.transform.localScale=Vector3.one*scale;
    }
}
