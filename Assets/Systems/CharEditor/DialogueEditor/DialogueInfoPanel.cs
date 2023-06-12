#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueInfoPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public static DialogueInfoPanel instance {get{return FindObjectOfType<DialogueInfoPanel>(true);}}
    public DialogueEditorNode selectedNode;
    public DialogueEditorLinesList linesList;
    public TMP_InputField keyField;
    public GameObject nodeInfo;
    void Awake()
    {
        nodeInfo.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (selectedNode==null){
            nodeInfo.SetActive(false);
        }
    }

    public void SelectNode(DialogueEditorNode node){
        DialogueEditorNode oldNode = selectedNode;
        selectedNode=node;
        node.UpdateFrameType();
        if (oldNode!=null)
            oldNode.UpdateFrameType();
            
        keyField.text=node.dialogue.key;
        linesList.UpdateWithDialogue(node.dialogue);
        nodeInfo.SetActive(true);
    }
    public void UpdateKey(string t){
        keyField.text=t;
        selectedNode.dialogue.key=t;
    }
    public void ImportFromClipboard(){
        selectedNode.dialogue.ImportFromString(System.Windows.Forms.Clipboard.GetText());
        linesList.UpdateWithDialogue(selectedNode.dialogue);
    }
    public void ExportToClipboard(){
        System.Windows.Forms.Clipboard.SetText(selectedNode.dialogue.ExportToString());
    }
}
#endif
