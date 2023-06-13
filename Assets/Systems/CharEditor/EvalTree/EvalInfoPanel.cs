#if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class EvalInfoPanel : MonoBehaviour
{
    public static EvalInfoPanel instance {get{return FindObjectOfType<EvalInfoPanel>(true);}}
    public GameObject nodeInfo,filterPanel,evalContent,strictSetting;
    public CharacterEditorFilterNode selectedNode;
    public TMP_InputField nameInputField,starRatingField,reviewField;
    public Toggle hasEvalToggle,returnsToggle;
    public TMP_Dropdown returnKeyDropdown;
    public List<string> returningKeys;

    void OnEnable()
    {
        returningKeys=CharacterEditor.currentCharacter.dialogue.allReturningKeys;
        
        returnKeyDropdown.options=new List<TMP_Dropdown.OptionData>();

        foreach(string s in returningKeys){
            returnKeyDropdown.options.Add(new TMP_Dropdown.OptionData(s));  
        }
        if (returnKeyDropdown.options.Count==0){
            returnKeyDropdown.options.Add(new TMP_Dropdown.OptionData("None")); 
        }
        SetSelectedNode(null);
        
    }
    // Update is called once per frame
    void Update()
    {
    }
    public void SetSelectedNode(CharacterEditorFilterNode cefn){
        CharacterEditorFilterNode oldNode = selectedNode;
        selectedNode=cefn;
        if (oldNode!=null){
            oldNode.UpdateFrameType();
        }
        if (selectedNode!=null){
            selectedNode.UpdateFrameType();

            nameInputField.text=cefn.node.name;
            starRatingField.text=cefn.node.evalNode.starRating.ToString();
            reviewField.text=cefn.node.evalNode.review;
            hasEvalToggle.isOn=cefn.node.hasEval;
            returnsToggle.isOn=cefn.node.evalNode.returns;

            
            filterPanel.SetActive(!cefn.isRoot);
            CharacterEditorFilterNode parent = cefn.transform.parent.GetComponentInParent<CharacterEditorFilterNode>(); 
            if (parent!=null && parent.isRoot)
            {
                strictSetting.SetActive(true);
                strictSetting.GetComponentInChildren<Toggle>().isOn=cefn.node.strict;
            }
            else{
                strictSetting.SetActive(false);
            }
            if (cefn.isRoot){
                hasEvalToggle.isOn=true;
            }

            bool found = false;
            for(int i = 0 ; i < returnKeyDropdown.options.Count;i++){
                if (returnKeyDropdown.options[i].text==cefn.node.evalNode.evaluationKey)
                {
                    found=true;
                    returnKeyDropdown.value=i;
                    break;
                }
            }
            if (!found)
                returnKeyDropdown.value=0;
                
            returnKeyDropdown.captionText.text=returnKeyDropdown.options[returnKeyDropdown.value].text;
            foreach (FilterButton fb in GetComponentsInChildren<FilterButton>(true)){
                fb.UpdateToggle();
            }
            nodeInfo.SetActive(true);
        }
        else{
            nodeInfo.SetActive(false);
        }
        
    }
    public void CreateRoot(){
        EvaluationEditor.CreateRoot();
    }
    public void AddChild(){
        EvaluationEditor.AddNodeChild(selectedNode);
    }
    public void DeleteNode(){
        if (selectedNode.isRoot)
            return;
        GameObject.Destroy(selectedNode.gameObject);
        SetSelectedNode(null);
    }
    public void UpdateName(){
        selectedNode.node.name=nameInputField.text;
    }



    public void ToggleEvaluation(bool value){
        if (value==false && selectedNode.isRoot)
        {  
            value=true;
            hasEvalToggle.isOn=true;
        }
        evalContent.SetActive(value);
        selectedNode.node.hasEval=value;
        selectedNode.UpdateFrameType();
    }
    public void ToggleReturns(bool value){
        selectedNode.node.evalNode.returns=value;
    }
    public void ToggleStrict(bool value){
        selectedNode.node.strict=value;
    }
    public void StarRatingUpdated(string value){
        if (value==""){
            value="0";
        }
        selectedNode.node.evalNode.starRating=float.Parse(value);
    }
    public void ReviewUpdated(string value){
        selectedNode.node.evalNode.review=value;
    }
    public void SetReturnKey(int value){
        selectedNode.node.evalNode.evaluationKey=returnKeyDropdown.options[value].text;
    }

}

#endif