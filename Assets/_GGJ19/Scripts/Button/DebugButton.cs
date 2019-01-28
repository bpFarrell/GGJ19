using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButton : BaseButton
{
    public MeshRenderer target;
    public Material glow;
    public Material noGlow;
    public MeshRenderer outputRenderer;
    private Material outputMat;
    public Color colorOn;
    public Color colorOff;
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

    private void Awake() {
        if (outputRenderer == null) return;
        outputMat = new Material(outputRenderer.material);
        outputRenderer.material = outputMat;
        outputMat.SetColor("_ColorOn", colorOn);
        outputMat.SetColor("_ColorOff", colorOff);
    }
    public override void Interact() {
        Debug.Log("PUSHED THE BUTTON!");
        buttonInteract = GetComponent<AudioSource>();
        buttonInteract.PlayOneShot(buttonPress, 1.0f);
        Toggle();
        if(rc== ResourceColor.PORTAL) {
            if (ResourceManager.Instance.yellowResource == 1 && DockedShip.instance != null) {
                CutSceneManager.Instance.ChangeState(CutSceneManager.GameState.ShipLeaving);
            }
        }
    }
    private void Toggle() {
        if (isRunning) {
            ResourceManager.Instance.generationState &= ~rc;
            PumpLocator.SetOff(rc);
        } else {
            ResourceManager.Instance.generationState = ResourceManager.Instance.generationState |rc;
            PumpLocator.SetOn(rc);
        }
    }
    public override void OnEnter() {
        if(target!=null)
            target.material = glow;
        isInRange = true;
    }
    public override void OnLeave() {
        if (target != null)
            target.material = noGlow;
        isInRange = false;
    }
    public void Update() {
        if (outputMat == null) return;
        float t = 1;
        switch (rc) {
            case ResourceColor.NONE:
                break;
            case ResourceColor.RED:
                t = ResourceManager.Instance.redResource;
                break;
            case ResourceColor.GREEN:
                t = ResourceManager.Instance.greenResource;
                break;
            case ResourceColor.BLUE:
                t = ResourceManager.Instance.blueResource;
                break;
            case ResourceColor.PORTAL:
                t = ResourceManager.Instance.yellowResource;
                break;
            default:
                break;
        }
        outputMat.SetFloat("_T", t);
    }
}
