#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

public class SpellEvaluationEditor : EditorWindow
{
    SpellEvaluationView treeView;
    InspectorView inspectorView;

    [MenuItem("Window/SpellEvaluationEditor")]
    public static void Open()
    {
        SpellEvaluationEditor wnd = GetWindow<SpellEvaluationEditor>();
        wnd.titleContent = new GUIContent("SpellEvaluationEditor");
    }

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        SpellEvaluationView sev = new SpellEvaluationView();
        root.Add(sev);
        sev.StretchToParentSize();
        // Import UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/SpellEvaluations/SEEditor/SpellEvaluationEditor.uxml");
        visualTree.CloneTree(root);
        

        // A stylesheet can be added to a VisualElement.
        // The style will be applied to the VisualElement and all of its children.
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/SpellEvaluations/SEEditor/SpellEvaluationEditor.uss");
        root.styleSheets.Add(styleSheet);

        treeView=root.Q<SpellEvaluationView>();
        inspectorView=root.Q<InspectorView>();

        treeView.OnNodeSelected=OnNodeSelectionChanged;
        OnSelectionChange();
    }


    private void OnSelectionChange() {
        SpellEvaluationTree tree = Selection.activeObject as SpellEvaluationTree;

        if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID())){
            treeView.PopulateView(tree);
        }
        
    }

    void OnNodeSelectionChanged(NodeView node){
        //inspectorView.UpdateSelection(node);
        
    }

}
#endif