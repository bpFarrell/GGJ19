using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairButton : BaseButton
{
    public System.Action<RepairButton> RemoveFromRoom;
    public void CleanUp() {
        Destroy(gameObject);
    }
    public override void Interact() {
        if( ResourceManager.Instance.redResource> Values.Hazards.REPAIRCOST) {
            ResourceManager.Instance.redResource -= Values.Hazards.REPAIRCOST;
        } else {
            return;
        }
        if (RemoveFromRoom != null)
            RemoveFromRoom(this);
        CleanUp();
    }

    public override void OnEnter() {

    }

    public override void OnLeave() {

    }

    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
