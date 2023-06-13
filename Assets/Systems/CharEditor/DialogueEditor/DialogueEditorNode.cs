#if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class DialogueEditorNode : MonoBehaviour
{
    public Dialogue dialogue;
    public TextMeshProUGUI label;
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<Image>().color = new Color(Random.value,Random.value,Random.value) * .25f;
        label=GetComponentInChildren<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        label.text=dialogue.key;
    }
    public void UpdateFrameType(){
        Color c = Color.white;
        if (DialogueInfoPanel.instance.selectedNode==this){
            c = Utility.ColorFromHex(0xFD0096);
        }
        transform.GetChild(1).GetComponent<Image>().color = c;
    }
    public void Delete(){
        GameObject.Destroy(gameObject);
    }
    public void Select(){
        DialogueInfoPanel.instance.SelectNode(this);
    }
    public Dialogue GetDialogue(){
        return dialogue;
    }
}
#endif
