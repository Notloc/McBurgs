using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsComponent : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    public bool IsConnectedToJoint { get; private set; }
    public bool IsLocked { get; private set; } = false;

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void SetConnectedToJoint(bool isConnectedToJoint)
    {
        IsConnectedToJoint = isConnectedToJoint;
    }

    public void SetLock(bool isLocked)
    {
        if (this.IsLocked == isLocked)
            return;

        IsLocked = isLocked;
        UpdateRigidbody(IsLocked);
    }

    private void UpdateRigidbody(bool isLocked)
    {
        Rigidbody.isKinematic = isLocked;
    }
}
