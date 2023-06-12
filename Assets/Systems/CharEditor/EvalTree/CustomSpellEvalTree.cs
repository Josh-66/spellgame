#if UNITY_STANDALONE_WIN || UNITY_EDITOR
using UnityEngine;
using System.Collections.Generic;
using System;
[Serializable]
public class CustomSpellEvalTree{
    public List<CustomFilterNode> filterNodes = new List<CustomFilterNode>(){new CustomFilterNode(){name="Default"}};
    public List<CustomEvalNode> evalNodes = new List<CustomEvalNode>();
    public SpellEvaluationTree GetTree(){
        SpellEvaluationTree tree = ScriptableObject.CreateInstance<SpellEvaluationTree>();
        foreach(CustomFilterNode node in filterNodes){
            tree.nodes.Add(node.GetNode());
        }
        for(int i = 0 ; i < filterNodes.Count;i++){
            foreach(int child in filterNodes[i].children){
                tree.nodes[i].next.Add(tree.nodes[child]);
            }
        }
        int genKey=0;
        for(int i = 0 ; i < filterNodes.Count;i++){
            if (filterNodes[i].hasEval){
                EvaluationNode en = filterNodes[i].evalNode.GetNode();
                if (en.evaluationKey=="None" || en.evaluationKey==""){ 
                    en.returns = false;
                }
                if (!en.returns){   
                    en.evaluationKey=genKey.ToString();
                    genKey++;
                }
                tree.nodes[i].AddChild(en);
                tree.nodes.Add(en);
            }
        }
        SERootNode rootnode =  ScriptableObject.CreateInstance<SERootNode>();
        foreach (SENode node in tree.nodes[0].next)
        {
            rootnode.AddChild(node);
        }
        rootnode.result=tree.nodes[0].result;

        tree.rootNode=rootnode;

        
        return tree;
    }
    public void DeleteNode(int i){
        DeleteNodeRC(i);
        ConsolidateTree();
    }
    void DeleteNodeRC(int i){
        if (i>=filterNodes.Count || i <0)
            return;
        if (filterNodes[i]==null)
            return;
        List<int> dChildren = filterNodes[i].children;
        filterNodes[i]=null;
        foreach(CustomFilterNode cfn in filterNodes){
            if (cfn!=null && cfn.children.Contains(i)){
                cfn.children.Remove(i);
            }
        }
        foreach(int c in dChildren){
            DeleteNode(c);
        }
    }
    public void ConsolidateTree(){
        for(int i = 0 ; i < filterNodes.Count ; i++){
            if (filterNodes[i]==null){
                filterNodes.RemoveAt(i);
                for (int j = 0; j < filterNodes.Count;j++)
                {
                    for (int k = filterNodes[j].children.Count-1; k >= 0 ;k--){
                        if (filterNodes[j].children[k] >i){
                            filterNodes[j].children[k]--;
                        }
                        else if (filterNodes[j].children[k]==i){
                            filterNodes[j].children.RemoveAt(k);
                        }
                    }
                }
                i--;
            }
        }
        
    }
}
[Serializable]
public class CustomFilterNode{
    public List<GlyphType> types = new List<GlyphType>();
    public List<int> children = new List<int>();
    public CustomEvalNode evalNode=new CustomEvalNode();
    public string name;
    public bool strict = false;
    public bool hasEval=false;
    public FilterNode GetNode(){
        FilterNode node;
        if (!strict) 
            node = ScriptableObject.CreateInstance<FilterNode>();
        else
            node = ScriptableObject.CreateInstance<BonusFilterNode>();
        node.filterTypes = new List<GlyphType>();
        foreach(GlyphType type in types){
            node.filterTypes.Add(type);
        }
        node.name=name;
        return node;
    }
}
[Serializable]
public class CustomEvalNode{
    public string evaluationKey;
    public bool returns;
    public float starRating;
    public string review;
    public EvaluationNode GetNode(){
        EvaluationNode node = ScriptableObject.CreateInstance<EvaluationNode>();
        node.evaluationKey=evaluationKey;
        node.returns=returns;
        node.starRating=starRating;
        node.review=review;
        return node;
    }
}
#endif