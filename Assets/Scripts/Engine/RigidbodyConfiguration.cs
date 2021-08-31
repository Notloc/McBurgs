using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyConfiguration
{
    private float mass;
    private float drag;
    private float angularDrag;
    private bool useGravity;
    private bool isKinematic;
    private RigidbodyInterpolation interpolation;
    private CollisionDetectionMode collisionDetectionMode;
    private RigidbodyConstraints contraints;

    public RigidbodyConfiguration() {}

    public RigidbodyConfiguration(Rigidbody rigidbody)
    {
        SaveConfiguration(rigidbody);
    }

    public void SaveConfiguration(Rigidbody rigidbody)
    {
        this.mass = rigidbody.mass;
        this.drag = rigidbody.drag;
        this.angularDrag = rigidbody.angularDrag;
        this.useGravity = rigidbody.useGravity;
        this.isKinematic = rigidbody.isKinematic;
        this.interpolation = rigidbody.interpolation;
        this.collisionDetectionMode = rigidbody.collisionDetectionMode;
        this.contraints = rigidbody.constraints;
    }

    public void ApplyConfiguration(Rigidbody rigidbody)
    {
        rigidbody.mass = this.mass;
        rigidbody.drag = this.drag;
        rigidbody.angularDrag = this.angularDrag;
        rigidbody.useGravity = this.useGravity;
        rigidbody.isKinematic = this.isKinematic;
        rigidbody.interpolation = this.interpolation;
        rigidbody.collisionDetectionMode = this.collisionDetectionMode;
        rigidbody.constraints = this.contraints;
    }
}
