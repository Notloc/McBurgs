using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsComponent : MonoBehaviour
{
    public Rigidbody Rigidbody { get; private set; }
    public RigidbodyOverrides RigidbodyOverrides {
        get {
            if (_rigidbodyOverrides == null)
                _rigidbodyOverrides = new RigidbodyOverrides(Rigidbody);
            return _rigidbodyOverrides;
        }
    }
    private RigidbodyOverrides _rigidbodyOverrides;

    public bool IsConnectedToJoint { get; private set; }
    public bool IsLocked { get => Rigidbody.isKinematic || IsConnectedToJoint; }

    private void Awake()
    {
        Rigidbody = GetComponent<Rigidbody>();
    }

    public void SetConnectedToJoint(bool isConnectedToJoint)
    {
        IsConnectedToJoint = isConnectedToJoint;
    }
}
