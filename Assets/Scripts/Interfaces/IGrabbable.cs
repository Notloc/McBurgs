using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable : IInteractable
{
    Rigidbody Rigidbody { get; }
    void ChangeRigidbody(Rigidbody newRigidbody);

    Vector3 GrabOffset { get; }
    Quaternion GrabRotation { get; }
    bool Locked { get; }

    void Lock();
    void Unlock();
}
