using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButton : BaseButton
{
    public MeshRenderer target;
    public Material glow;
    public Material noGlow;
    public ResourceColor rc = ResourceColor.NONE;
    public bool isInRange = false;
    public bool isRunning {
        get { return (ResourceManager.Instance.generationState & rc) == rc; }
    }
    public bool isExclusivlyRunning {
        get { return (ResourceManager.Instance.generationState == rc); }
    }

    public AudioClip buttonPress;
    AudioSource buttonInteract;

    public override void Interact() {
        Debug.Log("PUSHED THE BUTTON!");
        buttonInteract = GetComponent<AudioSource>();
        buttonInteract.PlayOneShot(buttonPress, 1.0f);
        Toggle();
    }
    private void Toggle() {
        if (isRunning) {
            ResourceManager.Instance.generationState &= ~rc; 
        } else {
            ResourceManager.Instance.generationState = ResourceManager.Instance.generationState |rc;

        }
    }
    public override void OnEnter() {
        target.material = glow;
        isInRange = true;
    }
    public override void OnLeave() {
        target.material = noGlow;
        isInRange = false;
    }
}
