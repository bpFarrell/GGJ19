using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    private MeshRenderer mr;
    private Vector4 values = new Vector4(0f,0f,0f,0f);

    private void Awake()
    {
        mr = GetComponent<MeshRenderer>();
        if (mr != null) mr.material.SetVector("_Resource", values);
    }
    void Update()
    {
        if (mr != null)
        {
            GetValues();
            mr.material.SetVector("_Resource", values);
        }
    }
    private void GetValues() {
        values[0] = ResourceManager.Instance.redResource;
        values[1] = ResourceManager.Instance.greenResource;
        values[2] = ResourceManager.Instance.blueResource;
    }
}
