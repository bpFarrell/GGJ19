using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class CameraEffectDriver : MonoBehaviour
{
    public AudioMixer master;
    AudioSource oxygenIndicator;
    public bool oxygenLow;
    public float effectonEQ;
    public float effectonSpeed;

    private void Start()
    {
        oxygenIndicator = GetComponent<AudioSource>();
    }
    public Material mat;
    private void OnRenderImage(RenderTexture source, RenderTexture destination) {
        Graphics.Blit(source, destination, mat);
    }
    private void Update() {
        if (ResourceManager.Instance == null)return;
        mat.SetFloat("_T", ResourceManager.Instance.greenResource * 2);
        if (ResourceManager.Instance.greenResource <= 0.4f) {
            effectonEQ = 1.0f - Mathf.InverseLerp(0.0f, 0.8f, ResourceManager.Instance.greenResource);
            oxygenIndicator.pitch = 1.0f + Mathf.InverseLerp(0.0f, 0.5f, ResourceManager.Instance.greenResource / 2);
            PlayHeartBeat();
        }
        else
        {
            master.SetFloat("MasterEQ", 1.0f);
        }
    }
    public void PlayHeartBeat()
    {
        {
            //Debug.Log("You're low on Oxygen!");
            oxygenLow = true;
            while (oxygenLow == true & oxygenIndicator.isPlaying == false)
            {
                //Debug.Log(effectonEQ);
                oxygenIndicator.Play();
                master.SetFloat("MasterEQ",effectonEQ);
            }
        }
    }
}

