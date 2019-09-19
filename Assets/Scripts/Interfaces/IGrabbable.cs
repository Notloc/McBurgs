using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable : IInteractable
{
    Vector3 GrabOffset { get; }
    bool Locked { get; }

    void Lock();
    void Unlock();
}
