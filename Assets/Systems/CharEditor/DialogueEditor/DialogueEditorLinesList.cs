#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.UI;
using TMPro;

public class DialogueEditorLinesList : MonoBehaviour
{
    public static DialogueEditorLinesList instance => FindObjectOfType<DialogueEditorLinesList>(true);
    public GameObject linePanel;
    public GameObject newLineButton;
    public List<DialogueEditorLine> lines = new List<DialogueEditorLine>();
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateWithDialogue(Dialogue d){
        foreach(DialogueEditorLine line in lines){
            if (line!=null)
                GameObject.Destroy(line.gameObject);
        }
        lines.Clear();
        foreach(Line l in d.lines){
            AddLine(l);
        }
    }

    public void AddLine(Line line){
        GameObject newLinePanel = Instantiate(linePanel);
        newLinePanel.transform.SetParent(transform);
        newLinePanel.transform.localScale=Vector3.one;
        DialogueEditorLine del = newLinePanel.GetComponent<DialogueEditorLine>();
        del.SetLine(line);
        newLineButton.transform.SetAsLastSibling();
        lines.Add(del);
        UpdateDialogueLines();
    }
    public void UpdateDialogueLines(){
        DialogueInfoPanel.instance.selectedNode.dialogue.lines=
            lines
            .Where(l=>l!=null)
            .OrderBy(l=>l.transform.GetSiblingIndex())
            .Select(l=>l.GetLine())
            .ToList();
    }
    public void NewLine(){
        AddLine(new Line(true,""));
    }
}
#endif