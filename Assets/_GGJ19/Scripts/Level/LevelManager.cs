using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LevelManager : SingletonBehaviour<LevelManager>
{
    public GameObject doorTriggerPrefab;
    //List<GameObject> doorTriggerPool; <- should use this for big random levels

    private List<RoomNode> s_rooms = null;
    public List<RoomNode> rooms {
        get { return s_rooms;  }
        private set { s_rooms = value; }
    }
    public void Initialize()
    {
        //doorTriggerPrefab = transform.Find("DoorTriggerPrefab").gameObject;
        //doorTriggerPrefab.SetActive(false);

        //if (doorTriggerPool == null) doorTriggerPool = new List<GameObject>();

        rooms = GetComponentsInChildren<RoomNode>().ToList();
        foreach (var room in rooms) {
            room.Initialize();
        }

        //Add door triggers
        //foreach (var room in rooms)
        //{
        //    //only add door triggers for top and right do prevent doubling up
        //    if (room.top != null)
        //    {
        //        GameObject newTrigger = Instantiate(doorTriggerPrefab);
        //        newTrigger.transform.SetParent(room.transform);
        //        newTrigger.transform.rotation = Quaternion.Euler(0, 90, 0);
        //        newTrigger.transform.position = (room.bounds.center + room.top.bounds.center) / 2;
        //        newTrigger.SetActive(true);
        //        DoorTrigger trig = newTrigger.GetComponent<DoorTrigger>();
        //        trig.doorOneSide = room;
        //        trig.doorOtherSide = room.top;
        //        Debug.Log("Added door trigger " + (trig.doorOneSide == null ? "null" : trig.doorOneSide.name) + " to " + (trig.doorOtherSide == null ? "null" : trig.doorOtherSide.name));
        //    }
        //    if (room.right != null)
        //    {
        //        GameObject newTrigger = Instantiate(doorTriggerPrefab);
        //        newTrigger.transform.SetParent(room.transform);
        //        newTrigger.transform.position = (room.bounds.center + room.right.bounds.center) / 2;
        //        newTrigger.SetActive(true);
        //        DoorTrigger trig = newTrigger.GetComponent<DoorTrigger>();
        //        trig.doorOneSide = room;
        //        trig.doorOtherSide = room.right;
        //        Debug.Log("Added door trigger " + (trig.doorOneSide == null ? "null" : trig.doorOneSide.name) + " to " + (trig.doorOtherSide == null ? "null" : trig.doorOtherSide.name));
        //    }
        //}
    }
    public void CreateHazard() {
        int index = Random.Range(0, rooms.Count);
        rooms[index].AddHazard(); 
        //choose room to create hazard in
    }
    public void Cleanup() {
        foreach (var room in rooms) {
            room.Cleanup();
        }
        rooms = null;
    }
    private void Update() {

        if (Time.frameCount % 120 == 0) {
            CreateHazard();
        }
    }
}

// Hazards: Break into class?
// Not used
class HazardWeight {
    int[] weights;

    public HazardWeight()
    {
        this.weights = new int[Values.Hazards.WEIGHTS]; ;
    }
}
