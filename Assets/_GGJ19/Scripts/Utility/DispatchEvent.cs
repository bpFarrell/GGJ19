
public abstract class DispatchEvent
{
    public virtual int priority { get { return 0; } }
    public virtual void Awake() { }
    public virtual void Start() { }
    public virtual void Update() { }
    public virtual void FixedUpdate() { }
    public virtual void OnGUI() { }
    public virtual void OnEnable() { }
    public virtual void OnPreCleanup() { }
    public virtual void OnDisable() { }
    public virtual void OnDestroy() { }
    public virtual void OnApplicationQuit() { }
    public virtual void OnApplicationPause(bool pause) { }
    public virtual void OnApplicationFocus(bool focus) { }
}