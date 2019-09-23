﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerComponent : MonoBehaviour, IPausable
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
