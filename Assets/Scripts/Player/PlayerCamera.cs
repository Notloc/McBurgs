using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerCamera : MonoBehaviour
{
    [SerializeField] PlayerControlState controlState = null;
    [SerializeField] Transform headTransform = null;

    private Rigidbody rigid;
    private Transform cameraT;
    private float upDownRotation;
    private float leftRightRotation;

    private void Start()
    {
        rigid = GetComponent<Rigidbody>();
        cameraT = Camera.main.transform;
    }

    private void LateUpdate()
    {
        if (controlState.CanLook)
        {
            TakeInput();
        }

        UpdateRotations();
        UpdateCameraPosition();
    }

    private void TakeInput()
    {
        upDownRotation -= Input.GetAxis("Mouse Y");
        leftRightRotation += Input.GetAxis("Mouse X");
    }

    private void UpdateRotations()
    {
        rigid.rotation = Quaternion.Euler(0f, leftRightRotation, 0f);
        headTransform.localRotation = Quaternion.Euler(upDownRotation, 0f, 0f);
    }

    private void UpdateCameraPosition()
    {
        cameraT.position = headTransform.position;
        cameraT.rotation = headTransform.rotation;
    }
}
