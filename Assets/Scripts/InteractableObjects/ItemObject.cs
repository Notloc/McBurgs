using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IGrabbable
{
    [Header("ItemObject")]
    [Header("Required References")]
    [SerializeField] protected Rigidbody Rigidbody;

    [Header("Options")]
    [SerializeField] Vector3 grabOffet;

    public Vector3 GrabOffset { get { return grabOffet; } }
    public bool Locked { get; protected set; }

    public void Interact()
    {}

    public virtual void Lock()
    {
        Rigidbody.constraints = RigidbodyConstraints.FreezeAll; 
        Locked = true;
        this.gameObject.SetLayerRecursively(LayerManager.HeldLayer);
    }

    public virtual void Unlock()
    {
        Rigidbody.constraints = RigidbodyConstraints.None;
        Locked = false;
        this.gameObject.SetLayerRecursively(LayerManager.InteractionLayer);
    }
}
