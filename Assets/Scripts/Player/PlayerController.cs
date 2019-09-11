using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerComponent
{
    [Header("Required References")]
    [SerializeField] Rigidbody targetBody;
    [SerializeField] Camera targetCamera;

    [Header("Movement Options")]
    [SerializeField] float movementForce = 100f;
    [SerializeField] float sprintMultiplier = 1.4f;
    [SerializeField] float maxMovementSpeed = 7f;

    [Header("Camera Options")]
    [SerializeField] float sensitivity = 2f;
    [SerializeField] float maxXRotation = 80f;
    [SerializeField] float minXRotation = -80f;

    private float cameraRotation = 0f;

    private void FixedUpdate()
    {
        Rotate();
        Move(Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        // Body rotation
        float yRotation = Input.GetAxis(ControlBindings.VIEW_INPUT_X);
        Quaternion deltaRotation = Quaternion.Euler(0f, yRotation, 0f);

        targetBody.rotation = deltaRotation * targetBody.rotation;


        // Camera rotation
        float deltaXRotation = Input.GetAxis(ControlBindings.VIEW_INPUT_Y);
        cameraRotation -= deltaXRotation;

        cameraRotation = Mathf.Clamp(cameraRotation, minXRotation, maxXRotation);

        targetCamera.transform.localRotation = Quaternion.Euler(cameraRotation, 0f, 0f);

    }

    private void Move(float deltaTime)
    {
        Vector3 movement = new Vector3(
            Input.GetAxis(ControlBindings.MOVEMENT_INPUT_X), 
            0f, 
            Input.GetAxis(ControlBindings.MOVEMENT_INPUT_Y
        ));
        movement *= movementForce;

        // Sprint
        if (Input.GetButton(ControlBindings.SPRINT))
            movement *= sprintMultiplier;

        targetBody.AddRelativeForce(movement);

        // Clamp velocity if needed
        if (targetBody.velocity.sqrMagnitude > maxMovementSpeed * maxMovementSpeed)
            targetBody.velocity = Vector3.ClampMagnitude(targetBody.velocity, maxMovementSpeed);
    }
}
