using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ItemObject : NetworkBehaviour, IGrabbable
{
    [Header("ItemObject")]
    [Header("Required References")]
    [SerializeField] new protected Rigidbody rigidbody;

    [Header("Options")]
    [SerializeField] Vector3 grabOffet;
    [SerializeField] Quaternion grabRotation = Quaternion.identity;
    [SerializeField] bool snapToHands = false;

    public Rigidbody Rigidbody { get { return rigidbody; } }
    public Vector3 GrabOffset { get { return grabOffet; } }
    public Quaternion GrabRotation { get { return grabRotation; } }
    public bool Locked { get; protected set; }

    public bool SnapToHands { get {return snapToHands; } }

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

    public void ChangeRigidbody(Rigidbody newRigid)
    {
        rigidbody = newRigid;
    }
}
