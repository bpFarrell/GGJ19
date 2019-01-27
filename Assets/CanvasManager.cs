using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class CanvasManager : SingletonBehaviour<CanvasManager>
{
    private Canvas s_canvasReference;
    private Canvas canvasReference {
        get { return canvasReference == null ? (s_canvasReference = GetComponent<Canvas>()) : s_canvasReference; }
    }
    private Graphic[] renderers;
    private List<Color> startColors = new List<Color>();
    private void Start()
    {
        Initialize();
    }
    public void Initialize() {
        renderers = GetComponentsInChildren<Graphic>();
        foreach (Graphic text in renderers) {
            startColors.Add(text.color);
        }
    }
    public void Cleanup() {
        for (int i = 0; i < renderers.Length; i++) {
            renderers[i].color = startColors[i];
        }
        startColors.Clear();
    }
    private void Update()
    {
        EverythingToClear();
    }
    ///////////////////////////////////////////////////////
    public void EverythingToClear() {
        for (int i = 0; i < renderers.Length; i++) {
            renderers[i].color = Color.Lerp(renderers[i].color, Color.clear, 0.2f);
        }
    }
    public void BlinkSubmit() {

    }
}
