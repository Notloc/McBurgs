using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour, IPausable
{
    public bool IsPaused { get; private set; }

    public void Pause()
    {
        this.enabled = false;
    }

    public void Unpause()
    {
        this.enabled = true;
    }
}
