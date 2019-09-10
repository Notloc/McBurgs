using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemObject : MonoBehaviour, IGrabbable
{
    [Header("Required References")]
    [SerializeField] ItemData itemData;
    [SerializeField] Rigidbody Rigidbody;

    public void Interact()
    {}

    public virtual void Lock()
    {
        Rigidbody.isKinematic = true;
    }

    public virtual void Unlock()
    {
        Rigidbody.isKinematic = false;
    }

}
