using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IGrabbable : IInteractable
{
    bool Locked { get; }

    void Lock();
    void Unlock();
}
