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

    [SyncVar]
    bool locked;
    public bool Locked
    {
        get { return locked; }
        protected set { locked = value; }
    }

    public bool SnapToHands { get {return snapToHands; } }

    // The interaction of a grabbable is to be grabbed.
    public void Interact() {}

    // Lock the object
    [Server]
    public virtual void Lock()
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeAll; 
        Locked = true;
        this.gameObject.layer = LayerManager.HeldLayer;

        RpcLock();
    }
    [ClientRpc]
    protected virtual void RpcLock()
    {
        rigidbody.constraints = RigidbodyConstraints.FreezeAll;
        Locked = true;
        this.gameObject.layer = LayerManager.HeldLayer;
    }

    // Unlock the object
    [Server]
    public virtual void Unlock()
    {
        rigidbody.constraints = RigidbodyConstraints.None;
        Locked = false;
        this.gameObject.layer = LayerManager.InteractionLayer;

        RpcUnlock();
    }
    [ClientRpc]
    protected virtual void RpcUnlock()
    {
        rigidbody.constraints = RigidbodyConstraints.None;
        Locked = false;
        this.gameObject.layer = LayerManager.InteractionLayer;
    }

    // Change the objects referenced rigidbody
    [Server]
    public void ChangeRigidbody(Rigidbody newRigid)
    {
        var identity = newRigid.GetComponent<NetworkIdentity>();
        if (!identity)
            return;

        rigidbody = newRigid;
        RpcChangeRigidbody(identity.netId);
    }
    [ClientRpc]
    private void RpcChangeRigidbody(NetworkInstanceId id)
    {
        var obj = ClientScene.objects[id];
        var newRigid = obj.GetComponent<Rigidbody>();
        rigidbody = newRigid;
    }
}
