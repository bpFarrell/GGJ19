using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : SingletonBehaviour<PlayerController>
{
    public float moveSpeed = 1;
    float moddedSpeed {
        get {
            float mult;
            float blue = ResourceManager.Instance.blueResource;
            if (blue > .9)
                mult = 1.1f;
            if (blue > .6)
                mult = 1.0f;
            if (blue > .3)
                mult = 0.9f;
            else
                mult = 0.75f;
            return moveSpeed  * mult;
        }
    }
    public float turnSpeed = 1;
    private float yPos;
    BaseButton currentButton;
    float moveBias = 0.01f;
    Rigidbody rb;

    public RoomNode currentRoom;
    public RoomNode previousRoom;
    public RoomNode nextRoom;

    Animator animator;


    void Awake() {
        animator = transform.Find("PlayerMesh").GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        yPos = transform.position.y;
    }

    void Start()
    {
        //LevelManager.Instance.Initialize();

        //if (LevelManager.Instance.rooms.Count != 0)
        //    currentRoom = LevelManager.Instance.rooms[0];
        
        Debug.Log("player current room is " + (currentRoom == null ? "null" : "not null, hooray!"));

    }

    enum Direction { Up, Down, Right, Left, None }
    Direction GetDirection(Vector3 v)
    {
        float value = (float)((Mathf.Atan2(v.z, v.x) / Mathf.PI) * 180f);
        if (value < 0) value += 360f;
        if (value > 45 && value < 135) return Direction.Up;
        if (value > 135 && value < 225) return Direction.Left;
        if (value > 225 && value < 315) return Direction.Down;
        return Direction.Right; //pretend error states are go right
    }

    private bool FuckYouIHateThis = false;
    bool UpdateCurrentRoom2()
    {
        if (LevelManager.Instance == null || LevelManager.Instance.rooms == null) return FuckYouIHateThis;
        //get room with closest center
        float dist = 10000000;
        RoomNode closest = null;
        foreach (RoomNode node in LevelManager.Instance.rooms)
        {
            float d = (transform.position - node.bounds.center).sqrMagnitude;
            if (d < dist)
            {
                dist = d;
                closest = node;
            }
        }

        if (closest == currentRoom) return false;
        currentRoom = closest;
        return true;
    }

    //return  true if room just changed
    // *** WE SHOULDN'T HAVE TO SCAN ALL ROOMS EACH UPDATE!!
    bool UpdateCurrentRoom()
    {
        foreach (RoomNode node in LevelManager.Instance.rooms)
        {
            if (node.bounds.Contains(transform.position))
            {
                if (currentRoom == node) return false;
                Debug.Log("player in room "+node.name);
                previousRoom = currentRoom;
                currentRoom = node;
                return true;
            }
        }
        if (currentRoom != null)
        {
            //we are transitioning to a new room, figure out which
            Direction dir = GetDirection(transform.position - currentRoom.bounds.center);
            if (dir == Direction.Up) nextRoom = currentRoom.top;
            else if (dir == Direction.Down) nextRoom = currentRoom.bottom;
            else if (dir == Direction.Left) nextRoom = currentRoom.left;
            else if (dir == Direction.Right) nextRoom = currentRoom.right;
            previousRoom = currentRoom;
            currentRoom = null;
            Debug.Log("Not in a room, going into space!");

            // Need to open doors of previous and next rooms
        }
        //Debug.Log("Not in a room, lost in space!");
        return false;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateCurrentRoom2();

        Vector3 pos = transform.position;
        pos.y = yPos;
        transform.position = pos;

        if (Input.GetButtonDown("Jump") && currentButton != null) {
            currentButton.Interact();
            animator.SetTrigger("Interact");
        }

        Vector3 last_pos = transform.position;
        Vector3 dir = GetDir();
        if (dir.magnitude == 0)
        {
            animator.SetFloat("Blend", 0);
            return;
        }
        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
        float faceScale = Vector3.Dot(transform.forward, dir.normalized);
        Vector3 targetDir = transform.forward * moddedSpeed * Time.deltaTime * faceScale;
        RaycastHit hit;
        if(Physics.Raycast(transform.position,targetDir,out hit)
            && (hit.distance < targetDir.magnitude)||(hit.distance< moveBias)
            && hit.distance!=0 && !hit.collider.isTrigger) {
                transform.position = hit.point + hit.normal * moveBias;
        } else {
            transform.position += targetDir;
        }

        float v = (transform.position - last_pos).magnitude / Time.deltaTime;
        animator.SetFloat("Blend", v / 5);
        
    }
    Vector3 GetDir() {

        Vector3 dir = Vector3.zero;
        dir.x = Input.GetAxis("Horizontal");
        dir.z = Input.GetAxis("Vertical");

        if (Input.GetKey("a")) {
            dir.x = -1;
        }
        if (Input.GetKey("d")) {
            dir.x = 1;
        }
        if (Input.GetKey("s")) {
            dir.z = -1;
        }
        if (Input.GetKey("w")) {
            dir.z = 1;
        }

        return dir;
    }
    private void OnTriggerEnter(Collider other) {
        BaseButton button = other.gameObject.GetComponent<BaseButton>();
        if (button == null) return;
        currentButton = button;
        button.OnEnter();
    }
    private void OnTriggerExit(Collider other) {
        BaseButton button = other.gameObject.GetComponent<BaseButton>();
        if (button == null) return;
        if (currentButton == button) {
            currentButton = null;
        }
        button.OnLeave();
    }
    private void OnTriggerStay(Collider other) {
        if (currentButton != null) return;
        BaseButton button = other.gameObject.GetComponent<BaseButton>();
        if (button == null) return;
        currentButton = button;
    }
}
