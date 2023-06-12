using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
public abstract class SENode : ScriptableObject
{
    [HideInInspector] public string guid;

    [HideInInspector] public Vector2 position;
    [HideInInspector]public List<SENode> next = new List<SENode>();
    [HideInInspector] public EvaluationNode result;
    public static string contextFolder="";
    public virtual void Initialize(){
        next = new List<SENode>();
        result=null;
    }
    public virtual void AddChild(SENode n){
        if (n is EvaluationNode){
            result=(EvaluationNode)n;
        }
        else
            next.Add(n);
    }
    public virtual void RemoveChild(SENode n){
        if (n is EvaluationNode){
            result=null;
        }
        else
            next.Remove(n);
    }
    public virtual bool MatchSpell(Spell spell){
        return false;
    }

    public List<SENode> GetNext(){
        return next;
    }
    public virtual List<SENode> GetChildren(){
        List<SENode> children = new List<SENode>(next);
        children.Add(result);
        return children;
    }
    public EvaluationNode GetResult(){
        return result;
    }

    #if UNITY_EDITOR
    public virtual void SetUI(NodeView nodeView)
    {
        nodeView.style.backgroundColor=GetColor();
        Foldout foldout = new Foldout();
        foldout.Add(new NodeInspectorView() {node = this});
        nodeView.extensionContainer.Add(foldout);
        nodeView.extensionContainer.Q<NodeInspectorView>().CreateInspector();
        
    }
    #endif
    public virtual Color GetColor(){
        return Color.grey;
    }

}