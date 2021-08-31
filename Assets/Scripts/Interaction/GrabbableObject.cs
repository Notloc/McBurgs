using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsComponent))]
public class GrabbableObject : MonoBehaviour
{
    public PhysicsComponent Physics { get; private set; }

    private void Awake()
    {
        Physics = GetComponent<PhysicsComponent>();
    }




}
