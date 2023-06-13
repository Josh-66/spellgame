#if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FilterButton : MonoBehaviour
{
    public GlyphType glyphType;
    Toggle toggle;
    // Start is called before the first frame update
    void Awake()
    {
        toggle=GetComponentInChildren<Toggle>();   
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void UpdateToggle(){
        toggle.isOn = EvalInfoPanel.instance.selectedNode.node.types.Contains(glyphType);
    }
    public void ValueChanged(bool newValue){
        if (newValue){
            if (!EvalInfoPanel.instance.selectedNode.node.types.Contains(glyphType))
                EvalInfoPanel.instance.selectedNode.node.types.Add(glyphType);
        }
        else{
            EvalInfoPanel.instance.selectedNode.node.types.Remove(glyphType);
        }
    }
}
#endif