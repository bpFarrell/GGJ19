using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class InteractAudioController : MonoBehaviour
{
    //private bool buttonPushed;
    private bool resourceChanged;
    private 

    // Start is called before the first frame update
    void Start()
    {
        GameObject resources = GameObject.Find("StatusPannel");
        ResourceManager debugButton = resources.GetComponent<ResourceManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (resourceChanged == true)
        {
            Debug.Log("Fire the button audio!");
        }
    }

    void OnInteract()
    {

    }
}
