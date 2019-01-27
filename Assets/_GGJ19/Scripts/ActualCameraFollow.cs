﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActualCameraFollow : MonoBehaviour
{
    public PlayerController player;
    Transform targetObject;
    public Vector3 offset;
    public float roomWeight = .05f;
    public float followSpeed = .95f;

    public Transform panoutTarget;
    public Animator animator;
    public RuntimeAnimatorController animController;
    public AnimationClip panOutAnimation;

    Vector3 target_pos;

    void Start()
    {
        if (panoutTarget != null)
        {
            animator = panoutTarget.gameObject.GetComponent<Animator>();
            animController = animator.runtimeAnimatorController;
            panOutAnimation = animController.animationClips[0];
        }

        targetObject = player.transform;
    }

    Vector3 GetRoomCenter()
    {
        if (player.currentRoom != null) return player.currentRoom.bounds.center;
        if (player.currentRoom == null && player.previousRoom != null && player.nextRoom != null)
        {
            //we are in an airlock between rooms
            if ((player.transform.position-player.previousRoom.bounds.center).sqrMagnitude < (player.transform.position - player.nextRoom.bounds.center).sqrMagnitude)
            {
                return player.previousRoom.bounds.center;
            } else return player.nextRoom.bounds.center;
        }
        return targetObject.position;

        //return new Vector3(5 * ((int)(targetObject.position.x / 5)), targetObject.position.y, 5 * ((int)(targetObject.position.z / 5)));

    }

    void Update()
    {
        Vector3 room_center = GetRoomCenter();
        //Debug.Log("room center: " + room_center + ", player pos: "+targetObject.position);

        float y = targetObject.position.y + offset.y;
        target_pos = new Vector3(Mathf.Lerp(room_center.x, targetObject.position.x + offset.x, roomWeight),
                                targetObject.position.y + offset.y,
                                Mathf.Lerp(room_center.z, targetObject.position.z + offset.z, roomWeight)
                        );
                    

        transform.position = Vector3.Lerp(transform.position, target_pos, followSpeed);
    }
}
