using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRotator : MonoBehaviour
{
    [SerializeField] Rigidbody rigid = null;
    [SerializeField] PlayerControlState controlState = null;

    private float xInput = 0f;
    private float yInput = 0f;
    private float spinInput = 0f;

    private Transform cameraT;

    private void Start()
    {
        cameraT = Camera.main.transform;
    }

    private void Update()
    {
        controlState.SetRotatingObject(Input.GetButton("Rotate"));
        xInput -= Input.GetAxis("Mouse X");
        yInput -= Input.GetAxis("Mouse Y");
        spinInput += Input.GetAxis("Horizontal");
    }

    private void FixedUpdate()
    {
        if (controlState.IsRotatingObject)
        {
            Quaternion inverseCamera = Quaternion.Inverse(cameraT.rotation);

            Quaternion newRot = (cameraT.rotation * Quaternion.Euler(yInput, spinInput, xInput) * inverseCamera) * rigid.rotation;
            rigid.MoveRotation(newRot);
        }

        xInput = 0f;
        yInput = 0f;
        spinInput = 0f;
    }

    private void OnDisable()
    {
        controlState.SetRotatingObject(false);
    }
}
