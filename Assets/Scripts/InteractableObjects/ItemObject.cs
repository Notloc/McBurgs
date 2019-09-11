﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IGrabbable
{
    [Header("Required References")]
    [SerializeField] protected Rigidbody Rigidbody;

    public bool Locked { get; protected set; }

    public void Interact()
    {}

    public virtual void Lock()
    {
        Rigidbody.isKinematic = true;
        Locked = true;
        this.gameObject.SetLayerRecursively(LayerManager.HeldLayer);
    }

    public virtual void Unlock()
    {
        Rigidbody.isKinematic = false;
        Locked = false;
        this.gameObject.SetLayerRecursively(LayerManager.DefaultLayer);
    }
}