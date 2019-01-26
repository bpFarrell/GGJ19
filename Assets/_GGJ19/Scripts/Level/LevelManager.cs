using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : SingletonBehaviour<LevelManager>
{
    private List<RoomNode> s_rooms = null;
    public List<RoomNode> rooms {
        get { return s_rooms;  }
        private set { s_rooms = value; }
    }

    public void Initialize() {
        rooms = GetComponentsInChildren<RoomNode>().ToList();
        foreach (var room in rooms) {
            room.Initialize();
        }
    }
    public void CreateHazard() {
        //choose room to create hazard in
    }
    public void Cleanup() {
        foreach (var room in rooms) {
            room.Cleanup();
        }
        rooms = null;
    }
}

// Hazards: Break into class?
class HazardWeight {
    int[] weights;

    public HazardWeight()
    {
        this.weights = new int[Values.Hazards.WEIGHTS]; ;
    }
}
