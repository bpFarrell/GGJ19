using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTrigger : BaseButton
{
    public RoomNode doorOneSide;
    public RoomNode doorOtherSide;

    public GameObject doorPanel1, doorPanel2; //these slide open and closed
    float slidePos; //0..1
    enum Sliding { None, Open, Closed }
    Sliding sliding;

    public override void OnEnter()
    {
        Debug.Log("Enter door trigger for " + (doorOneSide == null ? "null" : doorOneSide.name) + " to " + (doorOtherSide == null ? "null" : doorOtherSide.name));
        sliding = Sliding.Open;
    }

    public override void OnLeave()
    {
        Debug.Log("Leave door trigger for " + (doorOneSide == null ? "null" : doorOneSide.name) + " to " + (doorOtherSide == null ? "null" : doorOtherSide.name));
        sliding = Sliding.Closed;
    }

    public override void Interact() { }
    
}
