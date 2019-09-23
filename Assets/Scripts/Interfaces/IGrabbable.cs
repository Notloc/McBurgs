using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable : IInteractable
{
    Rigidbody Rigidbody { get; }

    Vector3 GrabOffset { get; }
    bool Locked { get; }

    void Lock();
    void Unlock();
}
