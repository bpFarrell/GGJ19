using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(ResourceManager))]
public class ResourceManagerEditor : Editor
{
    private static bool foldout = true;

    private void OnEnable() {
        SceneDebugDisplay.Enable();
    }

    public override void OnInspectorGUI() {
        DrawDefaultInspector();
        ResourceManager script = (ResourceManager)target;

        EditorGUILayout.LabelField("Resource Override");
        EditorGUI.indentLevel = 1;
        script.redResource = EditorGUILayout.FloatField("Red: ", script.redResource);
        script.blueResource = EditorGUILayout.FloatField("Blue: ", script.blueResource);
        script.greenResource = EditorGUILayout.FloatField("Green: ", script.greenResource);
        script.yellowResource = EditorGUILayout.FloatField("Yellow: ", script.yellowResource);
        EditorGUI.indentLevel = 0;

        if (foldout = EditorGUILayout.Foldout(foldout, "Debug")) {
            if (GUILayout.Button("Initialize")) {
                script.Initialize();
            }
            if (GUILayout.Button("Cleanup")) {
                script.Cleanup();
            }
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.EndDisabledGroup();
        }
    }
}
