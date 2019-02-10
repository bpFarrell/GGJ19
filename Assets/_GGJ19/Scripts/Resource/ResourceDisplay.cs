using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceDisplay : MonoBehaviour
{
    private MeshRenderer mr;
    private Vector4 values = new Vector4(0f,0f,0f,0f);
    private Vector4 charging = new Vector4(0f, 0f, 0f, 0f);

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
            mr.material.SetVector("_ResourceCharge", charging);
        }
    }
    private void GetValues() {
        if (ResourceManager.Instance == null) return;
        values[0] = ResourceManager.Instance.redResource;
        values[1] = ResourceManager.Instance.greenResource;
        values[2] = ResourceManager.Instance.blueResource;
        values[3] = ResourceManager.Instance.yellowResource;
        charging[0] = (ResourceManager.Instance.generationState & ResourceColor.RED) == ResourceColor.RED ? 1f : 0f;
        charging[1] = (ResourceManager.Instance.generationState & ResourceColor.GREEN) == ResourceColor.GREEN ? 1f : 0f;
        charging[2] = (ResourceManager.Instance.generationState & ResourceColor.BLUE) == ResourceColor.BLUE ? 1f : 0f;
        charging[3] = (ResourceManager.Instance.generationState & ResourceColor.PORTAL) == ResourceColor.PORTAL ? 1f : 0f;
    }
}
