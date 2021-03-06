﻿using UnityEngine;

public class GameManager : SingletonBehaviour<GameManager>
{
    public GameObject levelPrefab;
    public GameObject cutScenePrefab;
    public GameObject playerPrefab;

    private GameObject playerReference;
    private GameObject cutSceneReference;

    public CutSceneManager cutSceneManager {
        get { return CutSceneManager.Instance; }
    }
    public LevelManager levelManager {
        get { return LevelManager.Instance; }
    }
    public ResourceManager resourceManager {
        get { return ResourceManager.Instance; }
    }
    public CanvasManager canvasManager {
        get { return CanvasManager.Instance; }
    }
    private void Awake() {
        OnTransitionEnter += (x,y)=> {
            Initialize();
            levelManager.Initialize();
            resourceManager.Initialize();
        };
        OnPlayExit += (x)=> {
            Cleanup();
        };
    }
    private void Start() {
        state = GameState.MENU;
    }

    ///////////////////////////////////////////////////////
    public void Initialize() {
        if (levelManager == null && levelPrefab != null) Instantiate(levelPrefab);
        if (resourceManager == null) new GameObject("Resource Manager").AddComponent<ResourceManager>();
        if (playerReference == null && playerPrefab != null && PlayerController.Instance == null) playerReference = Instantiate(playerPrefab);
        if (cutSceneReference == null && cutSceneManager == null && cutScenePrefab != null) cutSceneReference = Instantiate(cutScenePrefab);
        CreateAmbiance();
    }
    public void Cleanup() {
        // When this breaks, instead of hoping cleanup works perfectly, just destroy and re-initialize
        if (levelManager != null) {
            levelManager.Cleanup();
            Destroy(levelManager.gameObject);
        }
        if (resourceManager != null) {
            resourceManager.Cleanup();
            Destroy(resourceManager.gameObject);
        }
        if(playerReference != null) {
            Destroy(playerReference);
        }
        if (cutSceneReference != null) {
            Destroy(cutSceneReference);
        }
        if(canvasManager != null) {
            canvasManager.Cleanup();
        }

        // Reset Game States
        state = GameState.MENU;
    }
    ///////////////////////////////////////////////////////
    private void CreateAmbiance() {
        return;
        GameObject go = GameObject.Find("AudioAmbiance(Clone)");
        if (go == null)
        {
            Instantiate(Resources.Load<GameObject>("AudioAmbiance"));
        }
    }

    ///////////////////////////////////////////////////////
    /// Main State Machine
    /// 

    public delegate void StateEnterDelegate(GameState state, GameState previous);
    public StateEnterDelegate OnMenuEnter;
    public StateEnterDelegate OnTransitionEnter;
    public StateEnterDelegate OnPlayEnter;
    public StateEnterDelegate OnEndEnter;
    public delegate void StateExitDelegate(GameState state);
    public StateExitDelegate OnMenuExit;
    public StateExitDelegate OnTransitionExit;
    public StateExitDelegate OnPlayExit;
    public StateExitDelegate OnEndExit;

    private GameState s_state = GameState.NULL;
    public GameState state {
        get { return s_state; }
        set {
            if (value == s_state) return;
            GameState prev = s_state;
            s_state = value;
            EnterState(s_state, prev);
        }
    }
    private void EnterState(GameState state, GameState prev) {
        switch (state)
        {
            case GameState.NULL:
                break;
            case GameState.MENU:
                if (OnMenuEnter != null) OnMenuEnter(state, prev);
                break;
            case GameState.PLAY:
                if (OnPlayEnter != null) OnPlayEnter(state, prev);
                break;
            case GameState.END:
                Application.Quit();
                if (OnEndEnter != null) OnEndEnter(state, prev);
                break;
            case GameState.TRANSITION:
                if (OnTransitionEnter != null) OnTransitionEnter(state, prev);
                break;
            default:
                break;
        }
        ExitState(prev);
    }
    private void ExitState(GameState state) {
        switch (state)
        {
            case GameState.NULL:
                break;
            case GameState.MENU:
                if (OnMenuExit != null) OnMenuExit(state);
                break;
            case GameState.PLAY:
                if (OnPlayExit != null) OnPlayExit(state);
                break;
            case GameState.END:
                if (OnEndExit != null) OnEndExit(state);
                break;
            case GameState.TRANSITION:
                if (OnTransitionExit != null) OnTransitionExit(state);
                break;
            default:
                break;
        }
    }
}
public enum GameState {
    NULL,
    MENU,
    TRANSITION,
    PLAY,
    END
}