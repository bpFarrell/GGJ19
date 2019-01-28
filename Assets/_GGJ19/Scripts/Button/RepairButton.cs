using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RepairButton : BaseButton
{
    public System.Action<RepairButton> RemoveFromRoom;
    Vector3 actaullyScale;
    bool isShrinking;
    float growSpeed = 3f;
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
        isShrinking = true;
    }

    public override void OnEnter() {

    }

    public override void OnLeave() {

    }
    private void Awake() {
        actaullyScale = transform.localScale;
        transform.localScale = Vector3.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
    }

    // Update is called once per frame
    void Update()
    {
        if (!isShrinking) {
            transform.localScale = Vector3.Lerp(transform.localScale, actaullyScale, Time.deltaTime * growSpeed);
        } else {
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime * growSpeed);
            if (transform.localScale.x < 0.01) {
                CleanUp();
            }
        }
    }
}
