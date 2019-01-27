using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DoorTrigger))]
public class DoorTriggerEditor : Editor
{
    private static bool foldout = true;

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();
        DoorTrigger script = (DoorTrigger)target;

        if (foldout = EditorGUILayout.Foldout(foldout, "Debug"))
        {
            if (GUILayout.Button("Initialize"))
            {
                script.Initialize();
            }
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.EndDisabledGroup();
        }
    }
}
