using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabRotator : MonoBehaviour
{
    [SerializeField] PlayerControlState controlState = null;
    private Transform cameraT;

    private void Start()
    {
        cameraT = Camera.main.transform;
    }

    private void Update()
    {
        controlState.SetRotatingObject(Input.GetButton("Rotate"));
        if (controlState.IsRotatingObject)
        {
            float xInput = Input.GetAxis("Mouse X");
            float yInput = Input.GetAxis("Mouse Y");
            float spinInput = Input.GetAxis("Horizontal");
            Quaternion inverseCamera = Quaternion.Inverse(cameraT.rotation);
            transform.rotation = (cameraT.rotation * Quaternion.Euler(yInput, spinInput, -xInput) * inverseCamera) * transform.rotation;
        }
    }

    private void OnDisable()
    {
        controlState.SetRotatingObject(false);
    }
}
