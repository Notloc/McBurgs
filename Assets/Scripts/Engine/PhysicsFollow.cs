using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PhysicsFollow : MonoBehaviour
{
    [SerializeField] Transform target = null;
    
    private Quaternion previousRotation = Quaternion.identity;
    private Rigidbody rigid;
    
    private void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void SetTarget(Transform target)
    {
        this.target = target;
        if (target)
        {
            previousRotation = target.rotation;
        }
    }

    private void FixedUpdate()
    {
        if (!target)
            return;

        Quaternion deltaRotation = target.rotation * Quaternion.Inverse(previousRotation);

        rigid.MovePosition(target.position);
        rigid.MoveRotation(deltaRotation * rigid.rotation);

        previousRotation = target.rotation;
    }
}
