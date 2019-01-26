using UnityEngine;
using UnityEditor;
using System;
using System.Linq;
using System.Reflection;
using System.Collections.Generic;

[CustomEditor(typeof(Dispatcher))]
public class DispatcherEditor : Editor
{
    //////////Public////////////

    //////////Private///////////
    List<FoldoutHelper> deList = new List<FoldoutHelper>();
    Dispatcher script;

    //////////Properties////////

    private void OnEnable()
    {
        EditorApplication.update += Update;
        Dispatcher script = (Dispatcher)target;
        deList = new List<FoldoutHelper>();

        MethodInfo method = typeof(Dispatcher).GetMethod("BuildEventList", BindingFlags.NonPublic | BindingFlags.Instance);

        FieldInfo events = typeof(Dispatcher).GetField("events", BindingFlags.NonPublic | BindingFlags.Instance);
        if (events != null)
        {
            List<DispatchEvent> dList = (List<DispatchEvent>)events.GetValue(script);
            if (dList.Count <= 0)
            {
                method.Invoke(script, null);
                dList = (List<DispatchEvent>)events.GetValue(script);
            }
            dList.ForEach(x => { deList.Add(new FoldoutHelper(x)); });
        }

        //deList = GetFoldoutHelperList();
    }
    private void OnDisable()
    {
        EditorApplication.update -= Update;
    }

    void Update()
    {
        Repaint();
    }

    public override void OnInspectorGUI()
    {
        EditorGUILayout.BeginVertical();

        foreach (FoldoutHelper helper in deList)
        {
            if ((helper.isOpen = EditorGUILayout.Foldout(helper.isOpen, helper.eventType.GetType().Name)))
            {
                FieldInfo[] fields = helper.eventType.GetType().GetFields(BindingFlags.NonPublic | BindingFlags.Public | BindingFlags.Instance | BindingFlags.Static);

                for (int i = 0; i < fields.Length; i++)
                {
                    DisplayField(fields[i], helper.eventType);
                }
            }
        }

        EditorGUILayout.EndVertical();
    }

    void DisplayField(FieldInfo field, object reference)
    {
        if (field == null || reference == null) return;
        var fieldType = field.FieldType;
        if (fieldType.IsSubclassOf(typeof(ValueType)))
        {
            EditorGUILayout.LabelField(field.Name, field.GetValue(reference).ToString());
        }
    }

    List<Type> GetDispatcherEventList()
    {
        Assembly asmbly = Assembly.GetAssembly(typeof(DispatchEvent));

        List<Type> classes = (from t in asmbly.GetTypes()
                              where (t.IsClass && t.IsSubclassOf(typeof(DispatchEvent)))
                              select t).ToList();

        return classes ?? new List<Type>();
    }

    //List<FoldoutHelper> GetFoldoutHelperList()
    //{
    //    List<Type> dispatchEventList = GetDispatcherEventList();
    //    List<FoldoutHelper> temp = new List<FoldoutHelper>();
    //
    //    dispatchEventList.ForEach((t => { temp.Add(new FoldoutHelper(t)); }));
    //
    //    return temp ?? new List<FoldoutHelper>();
    //}
    //
    //class FoldoutHelper
    //{
    //    public bool isOpen = true;
    //    public Type eventType;
    //    public FoldoutHelper(Type t)
    //    {
    //        eventType = t;
    //    }
    //}
    class FoldoutHelper
    {
        public bool isOpen = false;
        public DispatchEvent eventType;
        public FoldoutHelper(DispatchEvent t)
        {
            eventType = t;
        }
    }
}