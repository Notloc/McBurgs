using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IGrabbable
{
    [Header("ItemObject")]
    [Header("Required References")]
    [SerializeField] protected Rigidbody rigidbody;

    [Header("Options")]
    [SerializeField] Vector3 grabOffet;

    public Rigidbody Rigidbody { get { return rigidbody; } }
    public Vector3 GrabOffset { get { return grabOffet; } }
    public bool Locked { get; protected set; }

    public void Interact()
    {}

    public virtual void Lock()
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeAll; 
        Locked = true;
        this.gameObject.layer = LayerManager.HeldLayer;
    }

    public virtual void Unlock()
    {
        rigidbody.constraints = RigidbodyConstraints.None;
        Locked = false;
        this.gameObject.layer = LayerManager.InteractionLayer;
    }
}
