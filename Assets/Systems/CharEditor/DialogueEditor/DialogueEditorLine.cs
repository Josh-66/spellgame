#if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class  DialogueEditorLine : MonoBehaviour
{
    public TextMeshProUGUI orderNumber,speaker;
    public TMP_InputField lineTextField;
    public TMP_Dropdown typeDropdown,portraitDropdown;

    public GameObject textEditor,portraitEditor;
    public GameObject activeEditor;
    public Line line;

    // Start is called before the first frame update
    void Start()
    {
        textEditor.SetActive(false);
        portraitEditor.SetActive(false);
        UpdateValues();
    }
    public void SetLine(Line line){
        this.line=line;
        UpdateValues();

    }
    public Line GetLine(){
        return line;
    }

    // Update is called once per frame
    void Update()
    {
        
        orderNumber.text=transform.GetSiblingIndex().ToString();
    }
    void UpdateValues(){
        UpdateType((int)line.type);
        UpdatePortrait(line.value);
        SetSpeaker(line.customerSpeaking);
        UpdateLineText(line.text);
    }
    public void MoveUp(){
        if (transform.GetSiblingIndex()==0)
            return;
        transform.SetSiblingIndex(transform.GetSiblingIndex()-1);
        DialogueEditorLinesList.instance.UpdateDialogueLines();
    }
    public void MoveDown(){
        if (transform.GetSiblingIndex()>=transform.parent.childCount-2)
            return;
        transform.SetSiblingIndex(transform.GetSiblingIndex()+1);
        DialogueEditorLinesList.instance.UpdateDialogueLines();
    }
    public void Delete(){
        GameObject.Destroy(gameObject);
        DialogueEditorLinesList.instance.UpdateDialogueLines();
    }
    public void UpdateType(int type){
        line.type=(Line.Type)type;
        if (activeEditor!=null)
            activeEditor.SetActive(false);
        activeEditor=(line.type switch{
            Line.Type.text=>textEditor,
            Line.Type.portrait=>portraitEditor,
            _=>textEditor
        });
        activeEditor.SetActive(true);
        typeDropdown.value=type;
    }
    public void UpdatePortrait(int i){
        line.value=i;
        portraitDropdown.value=i;
    }
    public void ToggleSpeaker(){
        SetSpeaker(!line.customerSpeaking);
    }
    public void SetSpeaker(bool customerSpeaking){
        line.customerSpeaking=customerSpeaking;
        speaker.text = customerSpeaking ? "Customer:":"Player:";
    }
    public void UpdateLineText(string lineText){
        line.text=lineText;
        lineTextField.text=lineText;
    }
}
#endif