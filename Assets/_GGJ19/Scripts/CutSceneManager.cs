﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//public class CutSceneManager : MonoBehaviour
public class CutSceneManager : SingletonBehaviour<CutSceneManager>
{
    public GameObject scaffold;
    public GameObject otherShip;
    public GameObject panOutTarget;

    public GameObject hull;
    public Material hullMat;
    

    public enum CutSceneState { Menu, NormalPlay, ShipDocking, ShipLeaving, EndBadly, End }
    public CutSceneState gameState = CutSceneState.Menu;
    public CutSceneState currentState { get { return gameState; } }

    //pan out controls
    enum Panning { None, In, Out }
    Panning panningDir;
    float panOutTime;
    float panOutDuration = 2;
    Animator panAnimator;
    RuntimeAnimatorController animController;
    AnimationClip panOutAnimation;
    public float panDebugTime = -1;


    //docking state
    enum DockingPhase { PanOut, Dock, PanIn }
    DockingPhase dockingPhase;
    float dockingPanTime;
    public float dockingPanDuration = 2;
    Animator scaffoldAnimator;
    float scaffoldSpeed;

    //othership
    Animator otherShipAnimator;

    //flying off state
    float flyOffDuration;
    float flyOffTime;
    enum FlyOffPhase { Wait, RetractScaffold, FlyOff, Done }
    FlyOffPhase flyOffPhase;
    float scaffoldOffTime;
    public float scaffoldOffDuration = 1;

    void Awake()
    {
        GameManager.Instance.OnPlayEnter += ReallyStart;
        hull.SetActive(false);

        //set up pan stuff
        panAnimator = panOutTarget.gameObject.GetComponent<Animator>();
        animController = panAnimator.runtimeAnimatorController;
        panOutAnimation = animController.animationClips[0];

        //docking
        hullMat = hull.GetComponent<MeshRenderer>().material;

        scaffoldAnimator = scaffold.GetComponent<Animator>();
        scaffoldSpeed = scaffoldAnimator.speed;
        
        otherShipAnimator = otherShip.GetComponent<Animator>();
        
    }

    void ReallyStart(GameState state, GameState previous)
    {
        //gameState = CutSceneState.NormalPlay;

       
        
        //other ship
        flyOffDuration = 2;

        GameManager.Instance.OnPlayEnter -= ReallyStart;
    }

    private void OnEnable()
    {
        ChangeState(CutSceneState.ShipDocking);

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
             //toggle docking
            if (gameState != CutSceneState.ShipDocking) ChangeState(CutSceneState.ShipDocking);
            else if (gameState == CutSceneState.ShipDocking) ChangeState(CutSceneState.NormalPlay);
            //ChangeState((GameState)((int)gameState + 1) % System.Enum.GetValues(typeof(myenum)).Length);
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
        {
            //toggle ship goes on way
            if (gameState != CutSceneState.ShipLeaving) ChangeState(CutSceneState.ShipLeaving);
            else if (gameState == CutSceneState.ShipLeaving) ChangeState(CutSceneState.NormalPlay);
            //ChangeState((GameState)((int)gameState + 1) % System.Enum.GetValues(typeof(myenum)).Length);
        }


        //do this whatever scene
        if (panningDir != Panning.None)
        {
            if (panningDir == Panning.Out)
            {
                if (panOutTime < panOutDuration)
                {
                    if (panOutTime == 0) hull.SetActive(true);
                    panOutTime += Time.deltaTime;
                }
                else {
                    panOutTime = panOutDuration;
                    panningDir = Panning.None;
                }
            }
            else if (panningDir == Panning.In)
            {
                if (panOutTime > 0)
                {
                    panOutTime -= Time.deltaTime;
                }
                else {
                    panOutTime = 0;
                    panningDir = Panning.None;
                    hull.SetActive(false);
                }
            }
            panAnimator.Play("Pan out", -1, panOutTime / panOutDuration);
            Color color = hullMat.color;
            color.a = Mathf.Clamp((panOutTime / panOutDuration - .2f) * 5/4f, 0, 1);
            hullMat.color = color;
            hullMat.SetFloat("_Glossiness", color.a);
        }



        if (gameState == CutSceneState.ShipDocking)
        {
            //if (dockingPhase == DockingPhase.PanOut)
            //{
            //}

            if (panOutTime > panOutDuration * .75f)
            {
                if (dockingPhase == DockingPhase.PanOut) {
                    //we are panned out enough, start ship and scaffold animations
                    dockingPhase = DockingPhase.Dock;
                    scaffoldAnimator.Play("Scaffold Connect", -1, 0);
                    scaffoldAnimator.speed = 1;
                    otherShipAnimator.Play("Dock", -1, 0);
                    otherShipAnimator.speed = 1;
                }

                dockingPanTime += Time.deltaTime;
                if (dockingPanTime >= dockingPanDuration)
                {
                    ChangeState(CutSceneState.NormalPlay);
                    //EndOfDock();
                }
            }
        }

        if (gameState == CutSceneState.ShipLeaving)
        {
            if (panOutTime > panOutDuration * .75f)
            {
                if (flyOffPhase == FlyOffPhase.Wait)
                {
                    flyOffPhase = FlyOffPhase.RetractScaffold;
                    scaffoldAnimator.Play("Scaffold Disconnect", -1, 0);
                    scaffoldAnimator.speed = 1;
                    scaffoldOffTime = 0;
                }
            }
            if (flyOffPhase == FlyOffPhase.RetractScaffold)
            {
                scaffoldOffTime += Time.deltaTime;
                if (scaffoldOffTime > scaffoldOffDuration)
                {
                    flyOffPhase = FlyOffPhase.FlyOff;
                    otherShipAnimator.Play("FlyToPortal", -1, 0);
                    otherShipAnimator.speed = 1;
                }
            }
            else if (flyOffPhase == FlyOffPhase.FlyOff)
            {
                flyOffTime += Time.deltaTime;
                if (flyOffTime > flyOffDuration)
                {
                    GameManager.Instance.state = GameState.END;
                    //ChangeState(CutSceneState.ShipDocking);
                }
            }
        }


    }

    public bool IsPanning()
    {
        if (panOutTime != 0) return true;
        return false;
    }

    public Vector3 CameraTarget(out float weight, out Quaternion sceneCam)
    {
        weight = 1 - panOutTime / panOutDuration;
        sceneCam = panOutTarget.transform.rotation;
        return panOutTarget.transform.position;
    }

    public void ChangeState(CutSceneState newState)
    {
        Debug.Log("Change scene state to " + newState);
        if (newState == gameState) return;
        gameState = newState;

        if (newState == CutSceneState.ShipDocking)
        {
            panningDir = Panning.Out;
            //panOutTime = 0;

            dockingPanTime = 0;
            dockingPhase = DockingPhase.PanOut;

            scaffoldAnimator.Play("Scaffold Connect", -1, 0);
            scaffoldAnimator.speed = 0;
            otherShipAnimator.Play("Dock", -1, 0);
            otherShipAnimator.speed = 0;
        }
        else if (newState == CutSceneState.NormalPlay)
        {
            panningDir = Panning.In;
        }
        else if (newState == CutSceneState.ShipLeaving)
        {
            panningDir = Panning.Out;
            otherShipAnimator.Play("FlyToPortal", -1, 0);
            otherShipAnimator.speed = 0;
            flyOffTime = 0;
            flyOffPhase = FlyOffPhase.Wait;
        }
    }


    /// <summary>
    /// Called right after docking animation finishes
    /// </summary>
    void EndOfDock()
    {

    }
}
