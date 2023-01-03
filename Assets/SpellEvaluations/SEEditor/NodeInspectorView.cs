#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System;
public class NodeInspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<NodeInspectorView,VisualElement.UxmlTraits>{}

    Editor editor;
    public SENode node;
    public NodeInspectorView(){ 
        
    }
    public void CreateInspector(){
        Clear();
        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(node);
        IMGUIContainer container = new IMGUIContainer(() => {editor.OnInspectorGUI();});
        Add(container);
    }
    // internal void UpdateSelection(NodeView nodeView){
    //     Clear();

    //     UnityEngine.Object.DestroyImmediate(editor);
    //     editor = Editor.CreateEditor(nodeView.node);
    //     IMGUIContainer container = new IMGUIContainer(() => {editor.OnInspectorGUI();});
    //     Add(container);
    // }
}
#endif
