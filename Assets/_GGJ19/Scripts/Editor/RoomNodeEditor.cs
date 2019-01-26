using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(RoomNode))]
public class RoomNodeEditor : Editor
{
    private static bool foldout = false;

    public override void OnInspectorGUI()
    {
        RoomNode script = (RoomNode)target;

        if (foldout=EditorGUILayout.Foldout(foldout, "Debug"))
        {
            //if (GUILayout.Button("test"))
            //{
            //
            //}
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField("Left: ", script.left!=null? script.left.transform.name : "");
            EditorGUILayout.LabelField("Top: ", script.top!=null? script.top.transform.name : "");
            EditorGUILayout.LabelField("Right: ", script.right!=null? script.right.transform.name : "");
            EditorGUILayout.LabelField("Bottom: ", script.bottom!=null? script.bottom.transform.name : "");
            EditorGUI.EndDisabledGroup();
        }
    }
}
