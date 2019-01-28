using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(MeshRenderer))]
public class MeshRendererEditor : Editor
{
    private static bool foldout = true;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        MeshRenderer script = (MeshRenderer)target;

        if (foldout = EditorGUILayout.Foldout(foldout, "Debug"))
        {
            if (GUILayout.Button("New Material"))
            {
                Material mat = script.material;
                AssetDatabase.CreateAsset(mat, "Assets/New Material.mat");
                script.material = mat;
            }
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.EndDisabledGroup();
        }
    }
}
