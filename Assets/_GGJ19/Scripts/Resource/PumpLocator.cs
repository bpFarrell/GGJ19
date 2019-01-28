using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PumpLocator : MonoBehaviour
{
    public ResourceColor type;
    public static Dictionary<ResourceColor,PumpLocator> locators = new Dictionary<ResourceColor, PumpLocator>();
    private static List<PumpLocator> locatorList = new List<PumpLocator>();
    public Material mat;
    // Start is called before the first frame update
    private void Awake() {
        locators.Add(type, this);
        locatorList.Add(this);
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (mr == null)
            GetComponentInChildren<MeshRenderer>();
        mat = mr.material;
        SetThisOff();
    }
    public static void SetOn(ResourceColor type) {
        locators[type].SetThisOn();
    }
    public static void SetOff(ResourceColor type) {
        locators[type].SetThisOff();
    }
    public static Material GetMat(ResourceColor type) {
        return locators[type].mat;
    }
    public void SetThisOn() {
        mat.SetVector("_EnergyPower", Vector4.one);
    }
    public void SetThisOff() {
        mat.SetVector("_EnergyPower", Vector4.zero);
    }
}
