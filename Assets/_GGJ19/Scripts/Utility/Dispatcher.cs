using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

public class Dispatcher : MonoBehaviour
{
    private static Dispatcher _instance;
    public static Dispatcher instance { get { return _instance; } }
    private List<DispatchEvent> events = new List<DispatchEvent>();
    private List<DispatchEvent> updateEvents = new List<DispatchEvent>();
    private List<DispatchEvent> fixedUpdateEvents = new List<DispatchEvent>();
    private List<DispatchEvent> guiEvents = new List<DispatchEvent>();
    public delegate void DispatchCullEvent();
    public static event DispatchCullEvent OnDispatchEnable;
    public static event DispatchCullEvent OnDispatchDisable;

    private DispatchPlatform platform = DispatchPlatform.All;
    void OnEnable()
    {
        foreach (DispatchEvent de in events)
        {
            try
            {
                de.OnEnable();
            }
            catch (System.Exception e)
            {
                Debug.LogError("error in " + de.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " : " + e);
            }
        }
        if (OnDispatchEnable != null) OnDispatchEnable();
    }
    void OnDisable()
    {
        if (OnDispatchDisable != null) OnDispatchDisable();
        foreach (DispatchEvent de in events)
        {
            try { de.OnPreCleanup(); }
            catch (System.Exception e)
            {
                Debug.LogError("error in " + de.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " : " + e);
            }
        }
        foreach (DispatchEvent de in events)
        {
            try
            {
                de.OnDisable();
            }
            catch (System.Exception e)
            {
                Debug.LogError("error in " + de.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " : " + e);
            }
        }
    }
    void Awake()
    {
        _instance = this;
        BuildEventList();
        Debug.Log("Dispatcher: Awake");
        foreach (DispatchEvent de in events)
        {
            try
            {
                de.Awake();
            }
            catch (System.Exception e)
            {
                Debug.LogError("error in " + de.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " : " + e);
            }
        }
    }
    void Start()
    {
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        Screen.orientation = ScreenOrientation.Portrait;
        foreach (DispatchEvent de in events)
        {
            try
            {
                de.Start();
            }
            catch (System.Exception e)
            {
                Debug.LogError("error in " + de.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " : " + e);
            }
        }
    }

    void Update()
    {
        foreach (DispatchEvent de in updateEvents)
        {
            try
            {
                de.Update();
            }
            catch (System.Exception e)
            {
                Debug.LogError("error in " + de.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " : " + e);
            }
        }
    }
    void FixedUpdate()
    {
        foreach (DispatchEvent de in fixedUpdateEvents)
        {
            try
            {
                de.FixedUpdate();
            }
            catch (System.Exception e)
            {
                Debug.LogError("error in " + de.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " : " + e);
            }
        }
    }
    void OnGUI()
    {
        foreach (DispatchEvent de in guiEvents)
        {
            try
            {
                de.OnGUI();
            }
            catch (System.Exception e)
            {
                Debug.LogError("error in " + de.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " : " + e);
            }
        }
    }
    void OnDestroy()
    {
        foreach (DispatchEvent de in events)
        {
            try
            {
                de.OnDestroy();
            }
            catch (System.Exception e)
            {
                Debug.LogError("error in " + de.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " : " + e);
            }
        }
    }
    private void OnApplicationQuit()
    {
        foreach (DispatchEvent de in events)
        {
            try
            {
                de.OnApplicationQuit();
            }
            catch (System.Exception e)
            {
                Debug.LogError("error in " + de.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " : " + e);
            }
        }
    }
    private void OnApplicationPause(bool pause)
    {
        foreach (DispatchEvent de in events)
        {
            try
            {
                de.OnApplicationPause(pause);
            }
            catch (System.Exception e)
            {
                Debug.LogError("error in " + de.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " : " + e);
            }
        }
    }
    private void OnApplicationFocus(bool focus)
    {
        foreach (DispatchEvent de in events)
        {
            try
            {
                de.OnApplicationFocus(focus);
            }
            catch (System.Exception e)
            {
                Debug.LogError("error in " + de.GetType().Name + "." + MethodBase.GetCurrentMethod().Name + " : " + e);
            }
        }
    }
    void BuildEventList()
    {
        foreach (System.Type type in
            Assembly.GetAssembly(typeof(Dispatcher)).GetTypes()
            .Where(myType => myType.IsClass && !myType.IsAbstract && myType.IsSubclassOf(typeof(DispatchEvent))))
        {
            Debug.Log("Dispatcher: Found valid class '" + type.ToString() + "'");
            if (!IsValidForPlatform(type)) continue;
            DispatchEvent de = (DispatchEvent)System.Activator.CreateInstance(type);
            events.Add(de);
            if (IsOverrideMethod(type, "Update"))
                updateEvents.Add(de);
            if (IsOverrideMethod(type, "FixedUpdate"))
                fixedUpdateEvents.Add(de);
            if (IsOverrideMethod(type, "OnGUI"))
                guiEvents.Add(de);
        }
        events.Sort((a, b) => a.priority.CompareTo(b.priority));
        updateEvents.Sort((a, b) => a.priority.CompareTo(b.priority));
        fixedUpdateEvents.Sort((a, b) => a.priority.CompareTo(b.priority));
        guiEvents.Sort((a, b) => a.priority.CompareTo(b.priority));
    }
    bool IsValidForPlatform(System.Type type)
    {
        //object[] attribs = type.GetCustomAttributes(true);
        //foreach (object attrib in attribs)
        //{
        //    if (attrib.GetType() == typeof(DispatchSpecifyPlatform))
        //    {
        //        DispatchSpecifyPlatform dsp = (DispatchSpecifyPlatform)attrib;
        //        DispatchPlatform dp = dsp.platform;
        //        if ((dp & platform) == DispatchPlatform.None)
        //        {
        //            return false;
        //        }
        //        else
        //        {
        //            return true;
        //        }
        //    }
        //}
        return true;
    }
    bool IsOverrideMethod(System.Type type, string methodName)
    {
        return type.GetMethod(methodName).DeclaringType == type && !type.GetMethod(methodName).IsAbstract;
    }
}

[System.FlagsAttribute]
public enum DispatchPlatform
{
    None = 0,
    StandAlone = 1,
    Android = 2,
    IOS = 4,
    WebPlayer = 8,
    WebGL = 16,
    Mobile = Android | IOS,
    Web = WebPlayer | WebGL,
    NonWeb = All & (~Web),
    NonIOS = All & (~IOS),
    All = 31
}