using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyOverrides
{
    public struct RigidbodyOverrideField<T>
    {
        Action<T> setAction;
        T defaultValue;

        public RigidbodyOverrideField(T defaultValue, Action<T> setAction)
        {
            this.defaultValue = defaultValue;
            this.setAction = setAction;
        }

        public void Set(T value) { setAction.Invoke(value); }
        public void Clear() { setAction.Invoke(defaultValue); }
    }

    private Rigidbody rigidbody;
    private RigidbodyConfiguration defaultConfiguration;

    public RigidbodyOverrideField<float> Mass;
    public RigidbodyOverrideField<float> Drag;
    public RigidbodyOverrideField<float> AngularDrag;
    public RigidbodyOverrideField<bool> IsKinematic;
    public RigidbodyOverrideField<RigidbodyInterpolation> Interpolation;
    public RigidbodyOverrideField<CollisionDetectionMode> CollisionDetectionMode;
    public RigidbodyOverrideField<RigidbodyConstraints> Constraints;

    public RigidbodyOverrides(Rigidbody rigidbody)
    {
        this.rigidbody = rigidbody;
        defaultConfiguration = new RigidbodyConfiguration(rigidbody);

        Mass = new RigidbodyOverrideField<float>(rigidbody.mass, val => rigidbody.mass = val);
        Drag = new RigidbodyOverrideField<float>(rigidbody.drag, val => rigidbody.drag = val);
        AngularDrag = new RigidbodyOverrideField<float>(rigidbody.angularDrag, val => rigidbody.angularDrag = val);
        IsKinematic = new RigidbodyOverrideField<bool>(rigidbody.isKinematic, val => rigidbody.isKinematic = val);
        Interpolation = new RigidbodyOverrideField<RigidbodyInterpolation>(rigidbody.interpolation, val => rigidbody.interpolation = val);
        CollisionDetectionMode = new RigidbodyOverrideField<CollisionDetectionMode>(rigidbody.collisionDetectionMode, val => rigidbody.collisionDetectionMode = val);
        Constraints = new RigidbodyOverrideField<RigidbodyConstraints>(rigidbody.constraints, val => rigidbody.constraints = val);
    }

    public void ClearAll()
    {
        defaultConfiguration.Apply(rigidbody);
    }
}
