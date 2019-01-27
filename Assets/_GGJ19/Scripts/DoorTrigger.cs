using UnityEngine;

public class DoorTrigger : BaseButton
{
    public GameObject door;
    public Vector3 start;

    public AudioSource source;
    public AudioClip soundOpen;
    public AudioClip soundClose;

    private bool isInit = false;

    private float slidePos; //0..1

    enum Sliding {
        NONE,
        OPEN,
        CLOSED
    }
    Sliding sliding;

    public void Initialize() {
        start = door.transform.position;
        isInit = true;
    }
    public void Cleanup() {
        start = Vector3.zero;
        isInit = false;
    }
    public void Update() {
        
        switch (sliding)
        {
            case Sliding.NONE:
                door.transform.position = start;
                break;
            case Sliding.OPEN:
                door.transform.position = Vector3.Lerp(door.transform.position, start + (door.transform.right * 2), 0.08f);
                break;
            case Sliding.CLOSED:
                door.transform.position = Vector3.Lerp(door.transform.position, start, 0.08f);
                break;
            default:
                break;
        }
    }

    public override void OnEnter() {
        Debug.Log("Enter door trigger");
        source.PlayOneShot(soundOpen);
        sliding = Sliding.OPEN;
    }

    public override void OnLeave() {
        Debug.Log("Leave door trigger");
        source.PlayOneShot(soundClose);
        sliding = Sliding.CLOSED;
    }

    public override void Interact() { }
    
}
