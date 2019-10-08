using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : PlayerComponent
{
    [Header("Required References")]
    [SerializeField] Rigidbody playerRBody;
    [SerializeField] Collider playerCollider;
    [SerializeField] Camera targetCamera;
    [SerializeField] PhysicMaterial movingMaterial;
    [SerializeField] PhysicMaterial stationaryMaterial;

    [Header("Movement Options")]
    [SerializeField] float movementForce = 100f;
    [SerializeField] float sprintMultiplier = 1.4f;
    [SerializeField] float maxMovementSpeed = 7f;

    [Header("Camera Options")]
    [SerializeField] float sensitivity = 2f;
    [SerializeField] float maxXRotation = 80f;
    [SerializeField] float minXRotation = -80f;

    public bool LookEnabled = true;

    private float cameraRotation = 0f;

    private void Update()
    {
        if (!isLocalPlayer)
            return;

        Rotate();
    }

    private void FixedUpdate()
    {
        if (!isLocalPlayer)
            return;
        Move(Time.fixedDeltaTime);
    }

    private void Rotate()
    {
        if (LookEnabled == false)
            return;

        // Body rotation
        float yRotation = Input.GetAxis(ControlBindings.VIEW_INPUT_X);
        Quaternion deltaRotation = Quaternion.Euler(0f, yRotation, 0f);

        this.transform.rotation = deltaRotation * this.transform.rotation;


        // Camera rotation
        float deltaXRotation = Input.GetAxis(ControlBindings.VIEW_INPUT_Y);
        cameraRotation -= deltaXRotation;

        cameraRotation = Mathf.Clamp(cameraRotation, minXRotation, maxXRotation);

        targetCamera.transform.localRotation = Quaternion.Euler(cameraRotation, 0f, 0f);

    }

    private void Move(float deltaTime)
    {
        float xInput = Input.GetAxis(ControlBindings.MOVEMENT_INPUT_X);
        float yInput = Input.GetAxis(ControlBindings.MOVEMENT_INPUT_Y);

        if (xInput == 0 && yInput == 0)
            playerCollider.material = stationaryMaterial;
        else
            playerCollider.material = movingMaterial;

        Vector3 movement = new Vector3(xInput, 0f, yInput) * movementForce;

        // Sprint
        bool isSprinting = Input.GetButton(ControlBindings.SPRINT);
        if (isSprinting)
            movement *= sprintMultiplier;

        float maxSpeed = isSprinting ? maxMovementSpeed * sprintMultiplier : maxMovementSpeed;

        playerRBody.AddRelativeForce(movement);

        // Clamp velocity if needed
        // Y is seperated to allow normal gravity
        Vector3 velocity = playerRBody.velocity;
        float yVelocity = velocity.y;
        velocity.y = 0f;
        if (velocity.sqrMagnitude > maxSpeed * maxSpeed)
        {
            velocity = Vector3.ClampMagnitude(velocity, maxSpeed);
            velocity.y = yVelocity;
            playerRBody.velocity = velocity;
        }
    }
}
