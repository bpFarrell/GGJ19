using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugButton : BaseButton
{
    public MeshRenderer target;
    public Material glow;
    public Material noGlow;
    public bool isInRange = false;
    public override void Interact() {
        Debug.Log("PUSHED THE BUTTON!");
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
