using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutSceneManager : MonoBehaviour
{
    public enum GameState { Menu, NormalPlay, ShipDocking, ShipLeaving, EndBadly, End }
    public GameState gameState = GameState.Menu;

    //docking state
    float dockingPanTime;

    void Start()
    {
        gameState = GameState.NormalPlay;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            if (gameState == GameState.NormalPlay) ChangeState(GameState.ShipDocking);
            else if (gameState == GameState.ShipDocking) ChangeState(GameState.NormalPlay);
            //ChangeState((GameState)((int)gameState + 1) % System.Enum.GetValues(typeof(myenum)).Length);
        }

        if (gameState == GameState.ShipDocking)
        {

        }
    }

    public void ChangeState(GameState newState)
    {
        Debug.Log("Change scene state to " + newState);
        if (newState == gameState) return;
        gameState = newState;

        if (newState == GameState.ShipDocking)
        {
            dockingPanTime = 0;
        }
    }
}
