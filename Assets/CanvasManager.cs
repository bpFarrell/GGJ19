using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : SingletonBehaviour<CanvasManager>
{
    public Graphic submit;

    private enum State {
        IDLE,
        TRANSITION,
        NULL
    }
    private State state = State.NULL;
    private Canvas s_canvasReference;
    private Canvas canvasReference {
        get { return canvasReference == null ? (s_canvasReference = GetComponent<Canvas>()) : s_canvasReference; }
    }
    private Graphic[] renderers;
    private List<Color> startColors = new List<Color>();
    private void Awake()
    {
        GameManager.Instance.OnMenuEnter += (x, y) => {
            Initialize();
        };
        GameManager.Instance.OnMenuExit += (x) => {
            state = State.TRANSITION;
        };
    }
    public void Initialize() {
        renderers = GetComponentsInChildren<Graphic>();
        foreach (Graphic text in renderers) {
            startColors.Add(text.color);
        }
        state = State.IDLE;
        interimColor = submit != null ? submit.color : Color.white;
    }
    public void Cleanup() {
        for (int i = 0; i < renderers.Length; i++) {
            renderers[i].color = startColors[i];
        }
        startColors.Clear();
    }
    private void Update()
    {
        switch (state) {
            case State.NULL:
                break;
            case State.IDLE:
                if (Input.anyKeyDown) {
                    GameManager.Instance.state = GameState.PLAY;
                }
                BlinkSubmit();
                break;
            case State.TRANSITION:
                EverythingToClear();
                break;
            default:
                break;
        }
    }
    ///////////////////////////////////////////////////////
    public void EverythingToClear() {
        for (int i = 0; i < renderers.Length; i++) {
            renderers[i].color = Color.Lerp(renderers[i].color, Color.clear, 0.2f);
        }
    }
    private Color interimColor;
    private void BlinkSubmit() {
        if (submit == null) return;
        interimColor.a = Mathf.Sin(Time.time*1.8f)* .4f + .6f;
        submit.color = interimColor;
    }
}
