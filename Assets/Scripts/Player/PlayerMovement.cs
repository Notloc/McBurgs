using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    private Rigidbody rigid;

    [SerializeField] PlayerControlState controlState = null;
    [SerializeField] float movementForce = 10f;
    [SerializeField, Range(0f, 1f)] float antiGravityStrength = 0.9f;
    [SerializeField] Transform bodyTransform = null;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
    }

    public void Move(Vector2 input)
    {
        bool inputIsInDeadzone = input.sqrMagnitude < 0.05f;
        if (inputIsInDeadzone || !controlState.CanMove)
            return;

        if (input.sqrMagnitude > 1f)
            input = input.normalized;

        Vector3 force = CalculateMovementForce(input);
        rigid.AddForce(force);
    }

    private Vector3 CalculateMovementForce(Vector2 input)
    {
        Vector3 movementForce = new Vector3(input.x, 0f, input.y) * this.movementForce;
        Vector3 antiGravityForce = GetAntiGravityForce();
        return bodyTransform.rotation * (movementForce + antiGravityForce);
    }

    private Vector3 GetAntiGravityForce()
    {
        return Physics.gravity * -rigid.mass * antiGravityStrength;
    }
}
