using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class SERootNode : SENode
{
    [HideInInspector]
    public List<SENode> bonusNext = new List<SENode>();
    public override void AddChild(SENode n){
        if (n is EvaluationNode){
            result=(EvaluationNode)n;
        }
        if (n is BonusFilterNode){
            bonusNext.Add(n);
        }
        else
            next.Add(n);
    }
    public override void RemoveChild(SENode n){
        if (n is EvaluationNode){
            result=null;
        }
        if (n is BonusFilterNode){
            bonusNext.Remove(n);
        }
        else
            next.Remove(n);
    }
    public override List<SENode> GetChildren(){
        List<SENode> children = new List<SENode>(next);
        children.Add(result);
        children.AddRange(bonusNext);
        return children;
    }
}