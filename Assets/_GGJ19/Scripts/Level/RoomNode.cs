﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomNode : MonoBehaviour {
    public enum RoomType {
        RED,
        BLUE,
        GREEN,
        PORTAL,
        NOTHING
    }
    // Inspector Controls
    public RoomType type = RoomType.NOTHING;
    public GameObject wallPrefab;
    public GameObject doorPrefab;
    public Transform geometryContainer;
    public Transform hazardContainer;
    public Transform interactionContainer;
    // Material Controls
    private Color targetLights = Values.Colors.NORMAL_LIGHTS;
    private Color targetRoom = Values.Colors.NORMAL_ROOM;
    private float roomColorSpeed = 1;
    private Material mat;
    // LevelManager
    private LevelManager manager {
        get { return LevelManager.Instance; }
    }
    // Oxygen Level
    public float oxygenModifier {
        get { return Values.Oxygen.BASE + (hasNeededRepairs ? 0 : Values.Oxygen.BREACH); }
    }
    public bool hasNeededRepairs{
        get { return currentRepairs.Count != 0; }
    }
    [HideInInspector]
    public List<RepairButton> currentRepairs = new List<RepairButton>();
    // Asset Bounds
    [HideInInspector]
    public Bounds bounds;
    // Adjacent Rooms
    //   1
    // 0 * 2
    //   3
    [HideInInspector]
    public RoomNode[] adjacentNodes = new RoomNode[4];
    public RoomNode left {
        get { return adjacentNodes[0]; }
        private set { adjacentNodes[0] = value; }
    }
    public RoomNode top {
        get { return adjacentNodes[1]; }
        private set { adjacentNodes[1] = value; }
    }
    public RoomNode right {
        get { return adjacentNodes[2]; }
        private set { adjacentNodes[2] = value; }
    }
    public RoomNode bottom {
        get { return adjacentNodes[3]; }
        private set { adjacentNodes[3] = value; }
    }
    // CRUD kinda
    public void Initialize() {
        FindNeighboors();
        PrepMaterial();
        BuildWalls();
        GetBounds();
    }

    public void Cleanup()
    {
        adjacentNodes = new RoomNode[4];
        CleanWalls();
    }
    ////////////////////////////////////////////
    private void BuildWalls() {
        if (wallPrefab == null || doorPrefab == null) return;
        // Time to play...
        WallOrDoor(top, 0f);
        WallOrDoor(right, 90f);
        WallOrDoor(bottom, 180f);
        WallOrDoor(left, 270f);
    }
    private void PrepMaterial() {
        MeshRenderer mr = GetComponentInChildren<MeshRenderer>();
        mat = new Material(mr.material);
        mr.material = mat;
    }
    private void UpdateLights() {
        if (hasNeededRepairs) {
            targetLights = Values.Colors.EMERGENCY_LIGHTS;
            targetRoom = Values.Colors.EMERGENCY_ROOM;
            roomColorSpeed = Values.Hazards.POWER_DOWN_SPEED;
        } else {
            targetLights = Values.Colors.NORMAL_LIGHTS;
            targetRoom = Values.Colors.NORMAL_ROOM;
            roomColorSpeed = Values.Hazards.POWER_UP_SPEED;
        }
    }
    private void WallOrDoor(RoomNode go, float rotation) {
        GameObject edge;
        if (go == null)edge = Instantiate(wallPrefab, transform.position, Quaternion.Euler(0, rotation, 0), geometryContainer);
        else {
            DoorTrigger script = (edge =Instantiate(doorPrefab, transform.position, Quaternion.Euler(0, rotation, 0), transform)).GetComponentInChildren<DoorTrigger>();
            if(script != null) script.Initialize();
        }
        SetMaterials(edge);
    }
    private void SetMaterials(GameObject edge) {
        MeshRenderer[] mrs = geometryContainer.GetComponentsInChildren<MeshRenderer>();
        for(int x = 0; x < mrs.Length; x++) {
            mrs[x].material = mat;
        }
    }
    private void CleanWalls() {
        foreach (Transform wall in geometryContainer) {
            DoorTrigger script = wall.GetComponent<DoorTrigger>();
            if (script != null) script.Cleanup();
            Destroy(wall);
        }
    }
    private void GetBounds()
    {
        bounds = new Bounds(transform.position, Vector3.zero);
        foreach (MeshRenderer mr in GetComponentsInChildren<MeshRenderer>()) {
            if (mr.gameObject == gameObject) continue;
            bounds.Encapsulate(mr.bounds);
        }
    }
    public void FindNeighboors() {
        List<RoomNode> nearest = (from r in manager.rooms
                    where r != this && (transform.position - r.transform.position).magnitude < 10
                    orderby (transform.position - r.transform.position).magnitude
                    select r).Take(4).ToList();

        top = nearest.OrderByDescending(x => Vector3.Dot(Vector3.forward, (x.transform.position - transform.position).normalized))
            .TakeWhile(x=> Vector3.Dot(Vector3.forward, (x.transform.position - transform.position).normalized) > 0.9f).LastOrDefault();
        if(top!=null)nearest.Remove(top);

        right = nearest.OrderByDescending(x => Vector3.Dot(Vector3.right, (x.transform.position - transform.position).normalized))
            .TakeWhile(x=> Vector3.Dot(Vector3.right, (x.transform.position - transform.position).normalized) > 0.9f).LastOrDefault();
        if(right!=null)nearest.Remove(right);

        bottom = nearest.OrderByDescending(x => Vector3.Dot(-Vector3.forward, (x.transform.position - transform.position).normalized))
            .TakeWhile(x=> Vector3.Dot(-Vector3.forward, (x.transform.position - transform.position).normalized) > 0.9f).LastOrDefault();
        if(bottom!=null)nearest.Remove(bottom);

        left = nearest.OrderByDescending(x => Vector3.Dot(-Vector3.right, (x.transform.position - transform.position).normalized))
            .TakeWhile(x=> Vector3.Dot(-Vector3.right, (x.transform.position - transform.position).normalized) > 0.9f).LastOrDefault();
        if(left!=null)nearest.Remove(left);
    }
    public void AddHazard() {
        GameObject go = (GameObject)Instantiate ( Resources.Load("Breach"),hazardContainer);
        RepairButton rb = go.GetComponent<RepairButton>();
        rb.RemoveFromRoom = RemoveHazard;
        float nudge = 1.5f;
        go.transform.position = new Vector3(
            UnityEngine.Random.Range(bounds.min.x + nudge, bounds.max.x - nudge),
            -0.55f,
            UnityEngine.Random.Range(bounds.min.z + nudge, bounds.max.z - nudge));
        go.transform.Rotate(0, UnityEngine.Random.Range(0, 360), 0);
        currentRepairs.Add(rb);
        UpdateLights();
    }
    public void RemoveHazard(RepairButton rb) {
        currentRepairs.Remove(rb);
        UpdateLights();
    }
    private void Update() {
        Color temp = mat.GetColor("_LightColor");
        mat.SetColor("_LightColor", Color.Lerp(temp, targetLights, roomColorSpeed * Time.deltaTime));
        temp = mat.GetColor("_FullRoom");
        mat.SetColor("_FullRoom", Color.Lerp(temp, targetRoom, roomColorSpeed * Time.deltaTime));
    }
    //////////////////////////////////////////////////////////////
    ///Gizmos
    private void OnDrawGizmosSelected() {
        Gizmos.color = Color.blue;
        if (left != null) {
            Gizmos.DrawSphere(left.transform.position, .5f);
            Gizmos.DrawLine(transform.position, left.transform.position);
        }
        if (top != null) {
            Gizmos.DrawSphere(top.transform.position, .5f);
            Gizmos.DrawLine(transform.position, top.transform.position);
        }
        if (right != null) {
            Gizmos.DrawSphere(right.transform.position, .5f);
            Gizmos.DrawLine(transform.position, right.transform.position);
        }
        if (bottom != null) {
            Gizmos.DrawSphere(bottom.transform.position, .5f);
            Gizmos.DrawLine(transform.position, bottom.transform.position);
        }
        Gizmos.color = Color.magenta;
        Gizmos.DrawWireCube(bounds.center, bounds.size);
    }
}
