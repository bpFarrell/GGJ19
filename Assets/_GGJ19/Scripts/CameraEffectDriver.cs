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
        if (ResourceManager.Instance == null) {
            mat.SetFloat("_T", 2f);
            return;
        }
        mat.SetFloat("_T", Mathf.Pow( ResourceManager.Instance.greenResource,0.5f) * 2);

        if (ResourceManager.Instance.greenResource <= 0.4f) {
            effectonEQ = Mathf.Lerp(-0.4f, 1.0f, ResourceManager.Instance.greenResource + 0.6f);
            oxygenIndicator.pitch = 1.0f + Mathf.Lerp(1.7f, 0.0f, ResourceManager.Instance.greenResource + 0.6f);
            PlayHeartBeat();
        }
        else
        {
            //Debug.Log("Breathing easy!");
            master.SetFloat("MasterEQ", 1.0f);
            oxygenIndicator.Stop();
        }
    }
    public void PlayHeartBeat()
    {
        //Debug.Log("You're low on Oxygen!");
        master.SetFloat("MasterEQ", effectonEQ);
        oxygenLow = true;
        while (oxygenLow == true & oxygenIndicator.isPlaying == false)
            {
            //Debug.Log(effectonEQ);
            oxygenIndicator.Play();
            }

    }
}

