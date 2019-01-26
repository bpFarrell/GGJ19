using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairButton : BaseButton
{
    public override void Interact() {
        Destroy(gameObject);
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
