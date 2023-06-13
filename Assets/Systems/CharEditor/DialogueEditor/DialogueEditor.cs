#if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public class DialogueEditor : MonoBehaviour
{
    public static DialogueEditor instance {get{return FindObjectOfType<DialogueEditor>(true);}}
    public ContentSizeFitter sizeFitter;
    public GameObject entry,exit,returnings,chats;
    public GameObject node;
    // Start is called before the first frame update
    void Awake()
    {
    }
    private void OnDisable() {
        if (CharacterEditor.currentCharacter!=null)
            CharacterEditor.currentCharacter.dialogue=GetDialogue();    
    }

    // Update is called once per frame
    void Update()
    {
        if (sizeFitter.horizontalFit==ContentSizeFitter.FitMode.MinSize){
            sizeFitter.horizontalFit=ContentSizeFitter.FitMode.PreferredSize;
            sizeFitter.verticalFit=ContentSizeFitter.FitMode.MinSize;
        }
        else{
            sizeFitter.horizontalFit=ContentSizeFitter.FitMode.MinSize;
            sizeFitter.verticalFit=ContentSizeFitter.FitMode.PreferredSize;
        }
    }
    public static void LoadTree(){
        CharacterDialogue dialogue = CharacterEditor.currentCharacter.dialogue;
        instance.entry.GetComponentInChildren<DialogueEditorNode>().dialogue=dialogue.entry;
        instance.exit.GetComponentInChildren<DialogueEditorNode>().dialogue=dialogue.exit;
        foreach(DialogueEditorNode n in instance.chats.GetComponentsInChildren<DialogueEditorNode>()){
            GameObject.Destroy(n.gameObject);
        }
        foreach(DialogueEditorNode n in instance.returnings.GetComponentsInChildren<DialogueEditorNode>()){
            GameObject.Destroy(n.gameObject);
        }
        foreach(Dialogue d in dialogue.chats){
            instance.AddNode(instance.chats.transform,d);
        }
        foreach(Dialogue d in dialogue.returnings){
            instance.AddNode(instance.returnings.transform,d);
        }
        
    }
    public static CharacterDialogue GetDialogue(){
        CharacterDialogue dialouge = new CharacterDialogue();
        dialouge.entry = instance.entry.GetComponentInChildren<DialogueEditorNode>(true).GetDialogue();
        dialouge.chats = instance.chats.GetComponentsInChildren<DialogueEditorNode>(true).Select(i=>i.GetDialogue()).ToArray();
        dialouge.returnings = instance.returnings.GetComponentsInChildren<DialogueEditorNode>(true).Select(i=>i.GetDialogue()).ToArray();
        dialouge.exit = instance.exit.GetComponentInChildren<DialogueEditorNode>(true).GetDialogue();


        return dialouge;
    }
    public void AddNode(Transform parent,Dialogue dialogue){
        Transform newNode = GameObject.Instantiate(instance.node).GetComponent<RectTransform>();
        newNode.transform.SetParent(parent);
        newNode.transform.localScale=Vector3.one;
        newNode.transform.SetSiblingIndex(parent.childCount-2);
        if (dialogue!=null)
            newNode.GetComponentInChildren<DialogueEditorNode>().dialogue=dialogue;
        
    }
    public void AddNode(Transform parent){
        AddNode(parent,null);    
    }

    public void ImportAllFromClipboard(){
        CharacterEditor.currentCharacter.dialogue.ImportFromString(System.Windows.Forms.Clipboard.GetText());
        LoadTree();
    }
    public void ExportAllToClipboard(){
        CharacterEditor.currentCharacter.dialogue=GetDialogue();    
        System.Windows.Forms.Clipboard.SetText(CharacterEditor.currentCharacter.dialogue.ExportToString());
    }
}
#endif