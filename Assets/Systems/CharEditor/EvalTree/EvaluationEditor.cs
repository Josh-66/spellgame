#if UNITY_STANDALONE_WIN || (UNITY_EDITOR && !UNITY_WEBGL)
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EvaluationEditor : MonoBehaviour
{
    public static EvaluationEditor instance {get{return FindObjectOfType<EvaluationEditor>(true);}}
    public ContentSizeFitter sizeFitter;
    public RectTransform content;
    public GameObject rootTemplate,nodeTemplate;
    public int flips = 0;
    
    // Start is called before the first frame update
    void Awake()
    {
        // rootTemplate=GetComponentInChildren<CharacterEditorEvalRoot>(true).gameObject;
        // nodeTemplate=GetComponentInChildren<CharacterEditorFilterNode>(true).gameObject;
        // rootTemplate.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (instance.sizeFitter.horizontalFit==ContentSizeFitter.FitMode.MinSize){
            instance.sizeFitter.horizontalFit=ContentSizeFitter.FitMode.PreferredSize;
            instance.sizeFitter.verticalFit=ContentSizeFitter.FitMode.MinSize;
        }
        else{
            instance.sizeFitter.horizontalFit=ContentSizeFitter.FitMode.MinSize;
            instance.sizeFitter.verticalFit=ContentSizeFitter.FitMode.PreferredSize;
        }
    }
    public static GameObject CreateRoot(){
        GameObject newNode = GameObject.Instantiate<GameObject>(instance.rootTemplate);
        newNode.transform.SetParent(instance.content);
        newNode.transform.localScale=Vector3.one;
        newNode.SetActive(true);
        
        
        // newNode.GetComponent<CharacterEditorEvalRoot>().enabled=true;
        // newNode.GetComponentInChildren<CharacterEditorFilterNode>(true).enabled=true;
        // newNode.GetComponentInChildren<CharacterEditorFilterNode>(true).GetComponent<Image>().enabled=true;
        return newNode;
    }
    public static CharacterEditorFilterNode AddNodeChild(CharacterEditorFilterNode node){
        return CreateNode(node.transform.GetChild(0) as RectTransform);
    }
    public static CharacterEditorFilterNode CreateNode(RectTransform rt){
    
        GameObject newNode = GameObject.Instantiate<GameObject>(instance.nodeTemplate);
        newNode.transform.SetParent(rt);
        newNode.transform.localScale=Vector3.one;
        newNode.GetComponent<Image>().color = new Color(Random.value,Random.value,Random.value) * .25f;
        newNode.SetActive(true);
        newNode.GetComponent<CharacterEditorFilterNode>().node=new CustomFilterNode();
        
        // newNode.GetComponent<CharacterEditorFilterNode>().enabled=true;
        // newNode.GetComponent<Image>().enabled=true;
        
        return newNode.GetComponent<CharacterEditorFilterNode>();
    }
    public static void ClearTree(){
        
        foreach(Transform t in instance.content){
            GameObject.Destroy(t.gameObject);
        }
    }
    public static void LoadTree(){

        ClearTree();
        CustomSpellEvalTree tree = CharacterEditor.currentCharacter.spellEvalTree;
        
        
        GameObject root = CreateRoot();
        CharacterEditorFilterNode cefn = root.GetComponentInChildren<CharacterEditorFilterNode>();
        cefn.node=tree.filterNodes[0];
        cefn.isRoot=true;
        cefn.UpdateFrameType();
        CreateChildren(tree, cefn);

    }
    public static void CreateChildren(CustomSpellEvalTree t, CharacterEditorFilterNode cefn){
        foreach(int i in cefn.node.children){
            CharacterEditorFilterNode newNode = AddNodeChild(cefn);
            newNode.node=t.filterNodes[i];
            
            newNode.UpdateFrameType();
            CreateChildren(t,newNode);
        }
    }
    public static CustomSpellEvalTree GetTree(){
        CustomSpellEvalTree tree = new CustomSpellEvalTree();
        
        List<CharacterEditorFilterNode> editorNodes = new List<CharacterEditorFilterNode>();
        List<CustomFilterNode> filterNodes = new List<CustomFilterNode>();
        foreach(CharacterEditorEvalRoot root in instance.content.GetComponentsInChildren<CharacterEditorEvalRoot>()){
            foreach(CharacterEditorFilterNode node in root.GetComponentsInChildren<CharacterEditorFilterNode>()){
                CustomFilterNode cfn = node.node;//new CustomFilterNode();
                // cfn.name=node.node.name;
                // cfn.types=node.node.types;
                // cfn.strict=node.node.str;
                //Copy over types data
                editorNodes.Add(node);
                filterNodes.Add(cfn);

            }
        }
        for(int i = 0; i < editorNodes.Count ; i++){
            filterNodes[i].children.Clear();
            foreach (Transform t in editorNodes[i].connectorRt){
                filterNodes[i].children.Add(editorNodes.IndexOf(t.GetComponent<CharacterEditorFilterNode>()));
            }
        }
        tree.filterNodes=filterNodes;
        
        tree.evalNodes=new List<CustomEvalNode>();

        return tree;
    }
}
#endif