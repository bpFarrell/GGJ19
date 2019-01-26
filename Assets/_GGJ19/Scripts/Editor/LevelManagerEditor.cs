using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor {
    private static bool foldout = false;

    public override void OnInspectorGUI()
    {
        LevelManager script = (LevelManager)target;

        if(foldout=EditorGUILayout.Foldout(foldout, "Debug")) {
            if (GUILayout.Button("Init")) {
                script.Initialize();
            }
            if (GUILayout.Button("Cleanup")) {
                script.Cleanup();
            }
            EditorGUI.BeginDisabledGroup(true);
            if(script.rooms != null) EditorGUILayout.LabelField("rooms count: ", script.rooms.Count.ToString());
            EditorGUI.EndDisabledGroup();
        }
    }
}

/*
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(LevelManager))]
public class LevelManagerEditor : Editor {
    private static bool foldout = false;

    public override void OnInspectorGUI()
    {
        LevelManager script = (LevelManager)target;

        if(EditorGUILayout.Foldout(foldout, "Debug")) {
            if (GUILayout.Button("test")) {

            }
            EditorGUI.BeginDisabledGroup(true);

            EditorGUI.EndDisabledGroup();
        }
    }
}
*/
