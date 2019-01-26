using UnityEngine;

public class RoomNode : MonoBehaviour
{
    // AssetManager
    // Oxygen Level
    private float oxygenModifier = Values.Oxygen.Base;

    // Adjacent Rooms
    //   1
    // 0 * 2
    //   3
    public RoomNode[] adjacentNodes = new RoomNode[4];
    public RoomNode left
    {
        get
        {
            return adjacentNodes[0];
        }
    }
    public RoomNode top
    {
        get
        {
            return adjacentNodes[1];
        }
    }
    public RoomNode right
    {
        get
        {
            return adjacentNodes[2];
        }
    }
    public RoomNode bottom
    {
        get
        {
            return adjacentNodes[3];
        }
    }

    public void Cleanup()
    {
        oxygenModifier = Values.Oxygen.Base;
        adjacentNodes = new RoomNode[4];
    }
}
