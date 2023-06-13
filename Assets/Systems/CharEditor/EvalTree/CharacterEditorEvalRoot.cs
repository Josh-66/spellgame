#if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterEditorEvalRoot : MonoBehaviour
{
    public RectTransform rt,topNodert;
    void Awake()
    {
        rt=GetComponent<RectTransform>();
        topNodert=transform.GetChild(0).GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (topNodert==null){
            GameObject.Destroy(gameObject);
            return;
        }
        rt.sizeDelta=new Vector2(Mathf.Max(100,topNodert.sizeDelta.x),100);
    }
}
#endif