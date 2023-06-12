#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CharacterEditorFilterNode : MonoBehaviour
{
    // Start is called before the first frame update
    public RectTransform rt,connectorRt;
    public TextMeshProUGUI nameText;
    public CustomFilterNode node;
    public bool isRoot;
    public void Awake()
    {
        rt=GetComponent<RectTransform>();
        connectorRt=transform.GetChild(0).GetComponent<RectTransform>();
        nameText=transform.GetChild(2).GetComponent<TextMeshProUGUI>();
        UpdateFrameType();
    }
    public void UpdateFrameType(){
        Color c = Color.white;
        if (EvalInfoPanel.instance.selectedNode==this){
            c = Utility.ColorFromHex(0xFD0096);
        }
        if (node.hasEval){
            c *= Utility.ColorFromHex(0xAAAAFF);
        }
        if (node.strict){
            c *= Utility.ColorFromHex(0x666666);
        }
        transform.GetChild(1).GetComponent<Image>().color = c;
    }
    // Update is called once per frame
    void Update()
    {
        rt.sizeDelta=new Vector2(Mathf.Max(100,connectorRt.sizeDelta.x),100);
        nameText.text=node.name;
    }
    public void SetText(string s){
        node.name=s;
    }

    public void SelectThis(){
        EvalInfoPanel.instance.SetSelectedNode(this);
    }
}
#endif