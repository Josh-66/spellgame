#if UNITY_EDITOR
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using System;
public class InspectorView : VisualElement
{
    public new class UxmlFactory : UxmlFactory<InspectorView,VisualElement.UxmlTraits>{}

    Editor editor;
    public InspectorView(){}

    internal void UpdateSelection(NodeView nodeView){
        Clear();

        UnityEngine.Object.DestroyImmediate(editor);
        editor = Editor.CreateEditor(nodeView.node);
        IMGUIContainer container = new IMGUIContainer(() => {editor.OnInspectorGUI();});
        Add(container);
    }
}
#endif