using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabSystem : MonoBehaviour
{
    [SerializeField] GrabJoint grabJoint = null;
    [SerializeField] TargetingSystem targetingSystem = null;

    public bool HasGrabbedObject { get => grabJoint.HasGrabbedObject; }

    public void GrabObject()
    {
        if (grabJoint.HasGrabbedObject)
        {
            return;
        }

        GrabbableObject grabObj = targetingSystem.GetTargetComponent<GrabbableObject>();
        if (!grabObj || grabObj.Physics.IsLocked)
            return;

        grabJoint.GrabObject(grabObj);
    }

    public void DropObject()
    {
        grabJoint.DropObject();
    }
}
