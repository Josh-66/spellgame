#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using UnityEditor.Experimental.GraphView;
public class NodeView : Node
{
    public Action<NodeView> OnNodeSelected;

    public SENode node;
    public Port input;
    public Port output;
    public Port output2;
    public Port output3;
    public NodeView(SENode node){
        this.node = node;
        this.title=node.name;//.Replace("","");
        this.viewDataKey = node.guid;

        style.left = node.position.x;
        style.top= node.position.y;

        CreateInputPorts();
        CreateOutputPorts();


        node.SetUI(this);
        

        mainContainer.AddToClassList("ds-node__main-container");
        extensionContainer.AddToClassList("ds-node__extension-container");


        RefreshExpandedState();
    }

    private void CreateInputPorts(){
        // if (node is TextNode){
        //     input=InstantiatePort(Orientation.Horizontal,Direction.Input,Port.Capacity.Single,typeof(bool));
        // }else if (node is PortraitChangeNode){
        //     input=InstantiatePort(Orientation.Horizontal,Direction.Input,Port.Capacity.Single,typeof(bool));
        // }else if (node is ChoiceNode){
        //     input=InstantiatePort(Orientation.Horizontal,Direction.Input,Port.Capacity.Single,typeof(bool));
        // }

        input=InstantiatePort(Orientation.Horizontal,Direction.Input,Port.Capacity.Multi,typeof(bool));

        if (input!=null && !(node is SERootNode)){
            input.portName = "";
            inputContainer.Add(input);
        }
    }
    private void CreateOutputPorts(){
        

        if (node is EvaluationNode){

        }
        else if (node is SERootNode){
            output=InstantiatePort(Orientation.Horizontal,Direction.Output,Port.Capacity.Multi,typeof(bool));
            if (output!=null){
                output.portName = "Normal";
                outputContainer.Add(output);
            }

            output3 = InstantiatePort(Orientation.Horizontal,Direction.Output,Port.Capacity.Single,typeof(bool));
            if (output3!=null){
                output3.portName = "Bonus";
                outputContainer.Add(output3);
            }

            output2 = InstantiatePort(Orientation.Horizontal,Direction.Output,Port.Capacity.Single,typeof(bool));
            if (output2!=null){
                output2.portName = "Result";
                outputContainer.Add(output2);
            }


            
        }
        else{
            output=InstantiatePort(Orientation.Horizontal,Direction.Output,Port.Capacity.Multi,typeof(bool));
            if (output!=null){
                output.portName = "Next";
                outputContainer.Add(output);
            }
            
            output2 = InstantiatePort(Orientation.Horizontal,Direction.Output,Port.Capacity.Single,typeof(bool));
            if (output2!=null){
                output2.portName = "Result";
                outputContainer.Add(output2);
            }
        
        }
    }
    public override void SetPosition(Rect newPos){
        base.SetPosition(newPos);
        node.position.x=newPos.xMin;
        node.position.y=newPos.yMin;
    }

    public override void OnSelected(){
        base.OnSelected();
        if (OnNodeSelected!=null){
            OnNodeSelected.Invoke(this);
        }
    }

}
#endif