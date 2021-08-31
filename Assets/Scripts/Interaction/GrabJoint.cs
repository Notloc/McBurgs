using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabJoint : MonoBehaviour
{
    public GrabbableObject GrabbedObject { get; private set; }
    public bool HasGrabbedObject { get => GrabbedObject; }

    public void GrabObject(GrabbableObject grabObj)
    {
        if (!HasGrabbedObject)
        {
            GrabbedObject = grabObj;
            //GrabbedObject.Physics.KinematicallyAttach(transform);
        }
    }

    public void DropObject()
    {
        if (HasGrabbedObject)
        {
            GrabbedObject.transform.SetParent(null);
           // GrabbedObject.Physics.Detach();
            GrabbedObject = null;
        }
    }
}