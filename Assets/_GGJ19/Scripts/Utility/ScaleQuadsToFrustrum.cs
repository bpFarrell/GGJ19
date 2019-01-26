using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleQuadsToFrustrum : MonoBehaviour
{
    private List<Transform> quads = new List<Transform>();

    public void GatherQuads() {
        foreach (Transform quad in transform) {
            quads.Add(quad);
        }
    }
    public void ResizeQuadsToCamera() {
        if (quads.Count <= 0) return;
        foreach (Transform quad in quads) {
            float distance = (quad.position - Camera.main.transform.position).magnitude;
            float frustumHeight = 2.0f * distance * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
            quad.localScale = new Vector3(frustumHeight * Camera.main.aspect, frustumHeight, 1);
        }
    }
    public void OnDrawGizmosSelected() {
        //Gizmos.color = Color.blue;
        //float frustumHeight = 2.0f * 11 * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        //Gizmos.DrawCube(Camera.main.transform.position + (Camera.main.transform.forward * 11f), new Vector3(frustumHeight * Camera.main.aspect, .1f, frustumHeight));
    }
}
