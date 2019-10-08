using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class PlayerComponent : NetworkBehaviour, IPausable
{
    public bool IsPaused { get; private set; }

    public virtual void Pause()
    {
        this.enabled = false;
    }

    public virtual void Unpause()
    {
        this.enabled = true;
    }
}
