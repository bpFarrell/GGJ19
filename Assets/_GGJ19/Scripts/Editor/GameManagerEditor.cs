using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(GameManager))]
public class GameManagerEditor : Editor
{
    private static bool foldout = true;

    public override void OnInspectorGUI()
    {
        GameManager script = (GameManager)target;

        EditorGUILayout.LabelField("Required: ");
        EditorGUI.indentLevel = 1;
        script.levelPrefab = EditorGUILayout.ObjectField("Level Prefab", script.levelPrefab, typeof(GameObject), false) as GameObject;
        EditorGUI.indentLevel = 0;

        if (foldout = EditorGUILayout.Foldout(foldout, "Debug"))
        {
            if (GUILayout.Button("Initialize"))
            {
                script.Initialize();
            }
            if (GUILayout.Button("Cleanup"))
            {
                script.Cleanup();
            }
            EditorGUI.BeginDisabledGroup(true);
            EditorGUILayout.LabelField("LevelManager: ", script.levelManager != null ? script.levelManager.transform.name : "");
            EditorGUILayout.LabelField("ResourceManager: ", script.resourceManager != null ? script.resourceManager.transform.name : "");
            EditorGUI.EndDisabledGroup();
        }
    }
}