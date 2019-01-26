using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalLogic : BaseButton
{
    public MeshRenderer target;
    public Material glow;
    public Material noGlow;
    public Material targetProgress;
    public delegate void PortalEvent();
    public static PortalEvent OnPortalComplete;
    public bool isInRange = false;
    public bool isRunning;
    public float currentProgress;
    public float progressSpeed = 0.1f;
    public override void Interact() {
        Debug.Log("PUSHED THE BUTTON!");
        isRunning = true;
    }
    public override void OnEnter() {
        target.material = glow;
        isInRange = true;
    }
    public override void OnLeave() {
        target.material = noGlow;
        isInRange = false;
    }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (isRunning) {
            currentProgress += progressSpeed * Time.deltaTime;
            if (currentProgress >= 1) {
                if (OnPortalComplete != null)
                    OnPortalComplete();
                currentProgress = 0;
                isRunning = false;
            }
            targetProgress.SetFloat("_T", currentProgress);
        }
    }
}
