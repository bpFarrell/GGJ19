using UnityEngine;

public class DoorTrigger : BaseButton
{
    public GameObject door;
    public Vector3 start;

    public AudioSource soundOpen;
    public AudioSource soundClose;

    private float slidePos; //0..1

    enum Sliding {
        NONE,
        OPEN,
        CLOSED
    }
    Sliding sliding;

    public void Initialize() {
        start = door.transform.position;
    }
    private void Cleanup() {

    }
    public void Update() {
        switch (sliding)
        {
            case Sliding.NONE:
                door.transform.position = start;
                break;
            case Sliding.OPEN:
                door.transform.position = Vector3.Lerp(door.transform.position, start + (door.transform.right * 2), 0.4f);
                break;
            case Sliding.CLOSED:
                door.transform.position = Vector3.Lerp(door.transform.position, start, 0.7f);
                break;
            default:
                break;
        }
    }

    public override void OnEnter() {
        Debug.Log("Enter door trigger");
        sliding = Sliding.OPEN;
    }

    public override void OnLeave() {
        Debug.Log("Leave door trigger");
        sliding = Sliding.CLOSED;
    }

    public override void Interact() { }
    
}
