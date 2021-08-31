using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsFollow : MonoBehaviour
{
    [SerializeField] Rigidbody rigid = null;
    [SerializeField] Transform target = null;

    Quaternion previousRotation;

    private void Start()
    {
        previousRotation = target.rotation;
    }

    private void FixedUpdate()
    {
        Quaternion deltaRotation = target.rotation * Quaternion.Inverse(previousRotation);

        rigid.MovePosition(target.position);
        rigid.MoveRotation(rigid.rotation * deltaRotation);

        previousRotation = target.rotation;
    }
}
