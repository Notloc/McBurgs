using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerControlState : MonoBehaviour
{
    public bool CanLook { get => !IsRotatingObject; }
    public bool CanMove { get => !IsRotatingObject; }


    public bool IsRotatingObject { get; private set; }

    public void SetRotatingObject(bool isRotatingObject)
    {
        IsRotatingObject = isRotatingObject;
    }


}
