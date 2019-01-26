using UnityEngine;

public class RoomNode : MonoBehaviour
{
    // AssetManager
    // Oxygen Level
    private float oxygenLevel = 0.0f;

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


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
