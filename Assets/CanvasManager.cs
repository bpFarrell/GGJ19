using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasManager : SingletonBehaviour<CanvasManager>
{
    public Graphic submit;
    public Material spaceMaterial;

    private enum State {
        IDLE,
        TRANSITION,
        END,
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
        switch (state)
        {
            case State.NULL:
                break;
            case State.IDLE:
                if (Input.anyKeyDown) {
                    GameManager.Instance.state = GameState.TRANSITION;
                }
                BlinkSubmit();
                break;
            case State.TRANSITION:
                EverythingToClear();
                TravelToOrigin();
                // Move shit here
                break;
            case State.END:
                break;
            default:
                break;
        }
        DriveSpaceBackground();
    }


    ///////////////////////////////////////////////////////
    public void EverythingToClear() {
        for (int i = 0; i < renderers.Length; i++) {
            renderers[i].color = Color.Lerp(renderers[i].color, Color.clear, 0.2f);
        }
    }
    private Vector2 offset = Vector2.zero;
    private void DriveSpaceBackground() {
        offset.x += Time.deltaTime;
        spaceMaterial.SetTextureOffset("_MainTex", offset);
    }
    private Color interimColor;
    private void BlinkSubmit() {
        if (submit == null) return;
        interimColor.a = Mathf.Sin(Time.time*1.8f)* .4f + .6f;
        submit.color = interimColor;
    }
    private readonly Vector3 CAMERAORIGIN = new Vector3(0, 6, 0);
    private void TravelToOrigin() {
        Camera.main.transform.position = Vector3.Lerp(Camera.main.transform.position, CAMERAORIGIN, 0.025f);
        if((CAMERAORIGIN - Camera.main.transform.position).magnitude <= 0.5f) { // At destination
            GameManager.Instance.state = GameState.PLAY;
            state = State.END;
        }
    }
}
