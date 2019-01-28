using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualCameraFollow : MonoBehaviour
{
    public PlayerController player;
    Transform targetObject;
    public Vector3 offset = new Vector3(0,10,0);
    public float roomWeight = .2f;
    public float followSpeed = .2f;

    Quaternion defaultRotation;

    Transform panoutTarget;
    CutSceneManager cutSceneManager;

    Vector3 target_pos;

    void Awake()
    {
        GameManager.Instance.OnPlayEnter += ReallyStart;
    }

    void ReallyStart(GameState state, GameState previous)
    {
        defaultRotation = transform.rotation;

        if (cutSceneManager == null) cutSceneManager = CutSceneManager.Instance;
        if (panoutTarget == null) panoutTarget = cutSceneManager.transform.Find("PanOutTarget");


        targetObject = player.transform;
    }

    Vector3 GetRoomCenter()
    {
        if (player.currentRoom != null) return player.currentRoom.bounds.center;
        return targetObject.position;

        //-----old, when rooms don't butt right up against each other:
        //if (player.currentRoom != null) return player.currentRoom.bounds.center;
        //if (player.currentRoom == null && player.previousRoom != null && player.nextRoom != null)
        //{
        //    //we are in an airlock between rooms
        //    if ((player.transform.position-player.previousRoom.bounds.center).sqrMagnitude < (player.transform.position - player.nextRoom.bounds.center).sqrMagnitude)
        //    {
        //        return player.previousRoom.bounds.center;
        //    } else return player.nextRoom.bounds.center;
        //}
        //return targetObject.position;
    }



    void LateUpdate()
    {
        Vector3 room_center = GetRoomCenter();
        //Debug.Log("room center: " + room_center + ", player pos: "+targetObject.position);

        float y = targetObject.position.y + offset.y;
        target_pos = new Vector3(Mathf.Lerp(room_center.x, targetObject.position.x + offset.x, roomWeight),
                                targetObject.position.y + offset.y,
                                Mathf.Lerp(room_center.z, targetObject.position.z + offset.z, roomWeight)
                        );
        Quaternion target_rot = defaultRotation;

        if (cutSceneManager.IsPanning())
        {
            float weight = 0;
            Quaternion sceneRot;
            Vector3 scenecam = cutSceneManager.CameraTarget(out weight, out sceneRot);
            target_pos = Vector3.Lerp(scenecam, target_pos, weight);

            target_rot = Quaternion.Lerp(sceneRot, defaultRotation, weight);
        }

        transform.rotation = target_rot;
        transform.position = Vector3.Lerp(transform.position, target_pos, followSpeed);
    }
}
