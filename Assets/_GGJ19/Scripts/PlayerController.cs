using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 1;
    public float turnSpeed = 1;
    BaseButton currentButton;
    float moveBias = 0.01f;
    Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetButtonDown("Jump") && currentButton != null) {
            currentButton.Interact();
        }

        Vector3 dir = GetDir();
        if (dir.magnitude == 0) return;
        Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, Time.deltaTime * turnSpeed);
        float faceScale = Vector3.Dot(transform.forward, dir.normalized);
        Vector3 targetDir = transform.forward * moveSpeed * Time.deltaTime * faceScale;
        RaycastHit hit;
        if(Physics.Raycast(transform.position,targetDir,out hit)
            && (hit.distance < targetDir.magnitude)||(hit.distance< moveBias)
            && hit.distance!=0 && !hit.collider.isTrigger) {
                transform.position = hit.point + hit.normal * moveBias;
        } else {
            transform.position += targetDir;
        }
        
        
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
        currentButton = null;
        button.OnLeave();
    }
}
