#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.Experimental.GraphView;
using System.Linq;
using System;

public class SpellEvaluationView : GraphView
{
    public Action<NodeView> OnNodeSelected;
    public new class UxmlFactory : UxmlFactory<SpellEvaluationView,GraphView.UxmlTraits>{}
    SpellEvaluationTree spellEvaluationTree;

    public SpellEvaluationView(){
        Insert(0,new GridBackground());

        this.AddManipulator(new ContentZoomer());
        this.AddManipulator(new ContentDragger());
        this.AddManipulator(new SelectionDragger());
        this.AddManipulator(new RectangleSelector());

        
        styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Systems/SpellEvaluations/SEEditor/NodeStyleSheet.uss"));
        styleSheets.Add(AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/Systems/SpellEvaluations/SEEditor/SpellEvaluationEditor.uss"));
    }

    NodeView FindNodeView(SENode node){
        if (node == null)
            return null;
        return GetNodeByGuid(node.guid) as NodeView;
    }
    internal void PopulateView(SpellEvaluationTree spellEvaluation){
        this.spellEvaluationTree = spellEvaluation;
        spellEvaluation.clearNull();

        graphViewChanged -= OnGraphViewChanged;
        DeleteElements(graphElements);
        graphViewChanged += OnGraphViewChanged;


        if (spellEvaluation.rootNode == null){
            spellEvaluation.rootNode = spellEvaluation.CreateNode(typeof(SERootNode));
           // spellEvaluation.rootNode = spellEvaluation.CreateNode(typeof(SERootNode));
            EditorUtility.SetDirty(spellEvaluation);
            AssetDatabase.SaveAssets();
        }
        // Creates node view
        spellEvaluation.nodes.ForEach(n=>CreateNodeView(n));

        //Create edges
        spellEvaluation.nodes.ForEach(n => {
            if (n==null)
                return;
            var children = spellEvaluation.GetChildren(n);
            children.ForEach(c => {
                NodeView parentView = FindNodeView(n);
                NodeView childView = FindNodeView(c);
                if (parentView!=null && childView !=null){
                    if (childView.node is EvaluationNode){
                        Edge edge = parentView.output2.ConnectTo(childView.input);
                        AddElement(edge);
                    }
                    if (parentView.node is SERootNode && childView.node is BonusFilterNode){
                        Edge edge = parentView.output3.ConnectTo(childView.input);
                        AddElement(edge);
                    }
                    else{
                        Edge edge = parentView.output.ConnectTo(childView.input);
                        AddElement(edge);
                    }
                }
            });
        });
    }
    public override List<Port> GetCompatiblePorts(Port startPort, NodeAdapter nodeAdapter){
        return ports.ToList().Where(endPort=>
            endPort.direction!= startPort.direction &&
            endPort.node != startPort.node 
            ).ToList();
    }

    public override void BuildContextualMenu(ContextualMenuPopulateEvent evt){
        Vector2 mousePos = evt.localMousePosition;
        //base.BuildContextualMenu(evt);
        var types = TypeCache.GetTypesDerivedFrom<SENode>();
        foreach (var type in types){
            if (type.Name!="Node"){
                
                evt.menu.AppendAction($"{type.Name}", (a) => CreateNode(type,mousePos));
            }
        }
    }

    void CreateNode(System.Type type,Vector2 mousePosition){
        SENode node = spellEvaluationTree.CreateNode(type);
        
        node.position=viewTransform.matrix.inverse.MultiplyPoint(mousePosition);
        
        NodeView nodeView = CreateNodeView(node);
        
    }
    NodeView CreateNodeView(SENode node){
        NodeView nodeView = new NodeView(node);
        
        AddElement(nodeView);

        nodeView.OnNodeSelected=OnNodeSelected;


        return nodeView;
    }

    private GraphViewChange OnGraphViewChanged(GraphViewChange graphViewChange){
        if (graphViewChange.elementsToRemove != null){
            graphViewChange.elementsToRemove.ForEach(elem =>{
                NodeView nodeView = elem as NodeView;
                if (nodeView !=null){
                    spellEvaluationTree.DeleteNode(nodeView.node);
                }
                Edge edge = elem as Edge;
                if (edge!=null){
                    NodeView parentView = edge.output.node as NodeView;
                    NodeView childView = edge.input.node as NodeView;
                    spellEvaluationTree.RemoveChild(parentView.node,childView.node);
                }
            });
        }
        List<Edge> invalidEdges = new List<Edge>();
        if (graphViewChange.edgesToCreate!=null){
            graphViewChange.edgesToCreate.ForEach(edge =>{

                NodeView parentView = edge.output.node as NodeView;
                NodeView childView = edge.input.node as NodeView;

               
                spellEvaluationTree.AddChild(parentView.node,childView.node);
            });
        }
        if (invalidEdges.Count>0)
            graphViewChange.edgesToCreate.RemoveAll(edge => invalidEdges.Contains(edge));


        return graphViewChange;
    }
}
#endif