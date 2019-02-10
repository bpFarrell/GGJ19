using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalAnimator : MonoBehaviour
{
    public GameObject bigGO;
    public GameObject medGO;
    public GameObject smallGO;
    public float bigSpeed;
    public float medSpeed;
    public float smallSpeed;
    void Start()
    {
        
    }
    
    void Update()
    {
        bigGO.transform.Rotate(0, 0, bigSpeed * Time.deltaTime, Space.Self);
        medGO.transform.Rotate(0, 0, medSpeed * Time.deltaTime, Space.Self);
        smallGO.transform.Rotate(0, 0, smallSpeed * Time.deltaTime, Space.Self);
    }
}
