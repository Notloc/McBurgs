using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [SerializeField] PlayerMovement playerMovement = null;
    [SerializeField] GrabSystem grabSystem = null;

    private Vector2 movementInput;
    private bool grabFlag;

    private void Update()
    {
        //TakeInput();
        TakeFixedInput();
    }

    private void FixedUpdate()
    {
        PerformFixedInputs();
        ClearFixedInputs();
    }

    private void TakeFixedInput()
    {
        grabFlag = grabFlag || Input.GetButtonDown("Fire1");
        movementInput = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
    }

    private void PerformFixedInputs()
    {
        playerMovement.Move(movementInput);
        if (grabFlag)
        {
            if (grabSystem.HasGrabbedObject)
                grabSystem.DropObject();
            else
                grabSystem.GrabObject();
        }
    }

    private void ClearFixedInputs()
    {
        grabFlag = false;
    }
}
