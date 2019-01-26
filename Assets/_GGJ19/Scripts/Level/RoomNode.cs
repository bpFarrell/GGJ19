using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class RoomNode : MonoBehaviour
{
    public enum RoomType {
        RED,
        BLUE,
        GREEN,
        PORTAL,
        NOTHING
    }
    // Inspector Controls
    public RoomType type = RoomType.NOTHING;

    // LevelManager
    private LevelManager manager {
        get { return LevelManager.Instance; }
    }
    // Oxygen Level
    private float s_oxygen = Values.Oxygen.BASE;
    private float s_oxygenMod = 0f;
    public float oxygenModifier {
        get { return s_oxygen + s_oxygenMod; }
        private set { s_oxygenMod = value; }
    }
    // Asset Bounds
    public Bounds bounds;

    // Adjacent Rooms
    //   1
    // 0 * 2
    //   3
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
        GetBounds();
    }
    public void Cleanup()
    {
        oxygenModifier = Values.Oxygen.BASE;
        adjacentNodes = new RoomNode[4];
    }
    ////////////////////////////////////////////
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
                    where r != this 
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
