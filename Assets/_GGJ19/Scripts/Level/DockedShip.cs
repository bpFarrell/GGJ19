using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DockedShip : MonoBehaviour
{
    public static DockedShip instance;
    public delegate void ShipEvent(DockedShip currentShip);
    public static ShipEvent OnShipDock;
    public static ShipEvent OnShipUndock;
    public float teleportTime = 60;
    public float blueTicSec = 0;
    public float redTicSec = 0;
    public float greenTicSec = 0;
    private void OnEnable() {
        if (instance != null)
            Debug.LogError("MULTIPLE DOCKED SHIPS ERROR!!!! ");
        instance = this;
        if (OnShipDock != null)
            OnShipDock(this);
    }
    private void OnDisable() {
        instance = null;
        if (OnShipUndock != null)
            OnShipUndock(this);
    }
}
