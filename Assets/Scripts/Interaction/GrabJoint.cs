using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GrabJoint : MonoBehaviour
{
    [SerializeField] ConfigurableJointSettings jointSettings = new ConfigurableJointSettings();
    
    public GrabbableObject GrabbedObject { get; private set; }
    public bool HasGrabbedObject { get => GrabbedObject; }

    private ConfigurableJoint joint;

    public void GrabObject(GrabbableObject grabObj)
    {
        if (!HasGrabbedObject)
        {
            GrabbedObject = grabObj;
            CreateJoint(grabObj.Physics);
        }
    }

    private void CreateJoint(PhysicsComponent target)
    {
        joint = gameObject.AddComponent<ConfigurableJoint>();
        jointSettings.Apply(joint);

        joint.autoConfigureConnectedAnchor = false;
        joint.connectedAnchor = Vector3.zero;
        joint.anchor = Vector3.zero;

        joint.connectedBody = target.Rigidbody;
        target.RigidbodyOverrides.Interpolation.Set(RigidbodyInterpolation.Interpolate);
    }

    public void DropObject()
    {
        Destroy(joint);
        _DropObject();
    }

    private void _DropObject()
    {
        if (HasGrabbedObject)
        {
            GrabbedObject.Physics.SetConnectedToJoint(false);
            GrabbedObject.Physics.RigidbodyOverrides.Interpolation.Clear();
            GrabbedObject = null;
        }
    }

    private void OnJointBreak(float breakForce)
    {
        _DropObject();
    }
}