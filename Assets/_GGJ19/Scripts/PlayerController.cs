using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1;
    public float turnSpeed = 1;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 dir = GetDir();
        if (dir.magnitude == 0) return;
        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
        float faceScale = Vector3.Dot(transform.forward, dir.normalized);
        transform.position += transform.forward * moveSpeed * Time.deltaTime *faceScale;
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
}
