using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CreateAssetMenu()]
public class SpellEvaluationTree : ScriptableObject
{
    public SENode rootNode;
    public List<SENode> nodes = new List<SENode>();
    #if UNITY_EDITOR
    public SENode CreateNode(System.Type type){
        SENode node = ScriptableObject.CreateInstance(type) as SENode;
        node.Initialize();

        node.name = type.ToString();
        node.guid = GUID.Generate().ToString();
        nodes.Add(node);


        AssetDatabase.AddObjectToAsset(node,this);
        AssetDatabase.SaveAssets();
        return node;

    }

    public void DeleteNode(SENode node){
        nodes.Remove(node);
        AssetDatabase.RemoveObjectFromAsset(node);
        AssetDatabase.SaveAssets();

        
    }
    public void AddChild (SENode parent, SENode child){
        parent.AddChild(child);

    }
    public void RemoveChild (SENode parent, SENode child){
        parent.RemoveChild(child);
    }
    public List<SENode> GetChildren (SENode parent){
        return parent.GetChildren();
    }
    public void clearNull(){
        for (int i = nodes.Count-1;i>=0;i--){
            if (nodes[i]==null)
                nodes.RemoveAt(i);
        }
    }
    #endif 
    
    
    public EvaluationNode EvaluateSpell(Spell spell){
        SENode currentNode = rootNode;
        SERootNode root = (SERootNode)rootNode;
        List<SENode> nextOptions = root.next;
        if (spell.bonus!=GlyphType.Invalid){
            nextOptions=root.bonusNext;
        }
        
        EvaluationNode lastEval = null;
        bool deadEnd=false;
        while (!deadEnd){
            if (currentNode.result!=null)
                lastEval = currentNode.result;
            bool found = false;
            foreach(SENode se in nextOptions){
                if (se == null)
                    continue;//THIS IS BAD
                if (se.MatchSpell(spell)){
                    currentNode=se;
                    nextOptions=currentNode.GetNext();
                    found=true;
                    break;
                }
            }
            if (!found){
                deadEnd=true;
            }
        }            

        return lastEval;
    }

    public static SpellEvaluationTree Get(string name){
        return Resources.Load<SpellEvaluationTree>("SpellTrees/"+name);
    }
    

}