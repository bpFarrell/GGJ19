using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ScaleQuadsToFrustrum))]
public class ScaleQuadsToFrustrumEditor : Editor
{
    private static bool foldout = true;

    public override void OnInspectorGUI()
    {
        ScaleQuadsToFrustrum script = (ScaleQuadsToFrustrum)target;

        if (foldout = EditorGUILayout.Foldout(foldout, "Debug"))
        {
            if (GUILayout.Button("Get Quads")) {
                script.GatherQuads();
            }
            if (GUILayout.Button("Size Quads")) {
                script.GatherQuads();
                script.ResizeQuadsToCamera();
            }
            EditorGUI.BeginDisabledGroup(true);
            
            EditorGUI.EndDisabledGroup();
        }
    }
}
