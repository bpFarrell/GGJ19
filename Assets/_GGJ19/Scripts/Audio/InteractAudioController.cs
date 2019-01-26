using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class InteractAudioController : MonoBehaviour
{
    //private bool buttonPushed;
    BaseButton debugButton;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject button = GameObject.Find("Button");
        DebugButton debugButton = button.GetComponent<DebugButton>();
        debugButton.Interact();
        {
            Debug.Log("Fire the button audio!");
        }
    }

    void OnInteract()
    {

    }
}
