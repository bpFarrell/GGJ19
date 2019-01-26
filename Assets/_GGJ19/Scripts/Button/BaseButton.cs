using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseButton : MonoBehaviour
{
    public abstract void OnEnter();
    public abstract void OnLeave();
    public abstract void Interact();

}
