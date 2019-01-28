using UnityEditor;
using UnityEngine;

public class SceneDebugDisplay : EditorWindow
{
    [MenuItem("Window/Scene GUI/Enable")]
    public static void Enable()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
        SceneView.onSceneGUIDelegate += OnScene;
        Debug.Log("Scene GUI : Enabled");
    }

    [MenuItem("Window/Scene GUI/Disable")]
    public static void Disable()
    {
        SceneView.onSceneGUIDelegate -= OnScene;
        Debug.Log("Scene GUI : Disabled");
    }

    private static void OnScene(SceneView sceneview)
    {
        Handles.BeginGUI();
        EditorGUILayout.LabelField("Red Resource: ", ResourceManager.Instance != null ? ResourceManager.Instance.redResource.ToString() : "");
        EditorGUILayout.LabelField("Blue Resource: ", ResourceManager.Instance != null ? ResourceManager.Instance.blueResource.ToString() : "");
        EditorGUILayout.LabelField("Green Resource: ", ResourceManager.Instance != null ? ResourceManager.Instance.greenResource.ToString() : "");

        Handles.EndGUI();
    }
}