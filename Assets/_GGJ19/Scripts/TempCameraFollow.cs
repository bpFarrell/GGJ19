using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCameraFollow : MonoBehaviour
{
    public Transform target;
    public Vector3 offset;
    // Start is called before the first frame update
    private void Awake() {
        offset = transform.position- target.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = target.position + offset;
    }
}
