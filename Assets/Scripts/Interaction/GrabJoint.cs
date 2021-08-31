using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabJoint : MonoBehaviour
{
    [Header("Spring Joint")]
    [SerializeField] ConfigurableJointSettings configurableJointSettings = new ConfigurableJointSettings();

    [Header("Connected Rigidbody")]
    [SerializeField] float rigidbodyDrag = 5f;
    [SerializeField] float rigidbodyAngularDrag = 2f;

    public GrabbableObject GrabbedObject { get; private set; }
    public bool HasGrabbedObject { get => GrabbedObject; }

    private ConfigurableJoint configJoint;
    private RigidbodyConfiguration cachedRigidbodyConfig = new RigidbodyConfiguration();

    public void GrabObject(GrabbableObject grabObj)
    {
        if (!HasGrabbedObject)
        {
            GrabbedObject = grabObj;    
            CreateSpringJointWithTarget(grabObj);
        }
    }

    private void CreateSpringJointWithTarget(GrabbableObject grabObj)
    {
        Rigidbody targetRigid = grabObj.Physics.Rigidbody;
        ApplySpringRigidbodySettings(targetRigid);

        configJoint = gameObject.AddComponent<ConfigurableJoint>();
        configurableJointSettings.Apply(configJoint, targetRigid.mass);
        configJoint.connectedBody = targetRigid;
    }

    private void ApplySpringRigidbodySettings(Rigidbody rigidbody)
    {
        cachedRigidbodyConfig.SaveConfiguration(rigidbody);
        rigidbody.drag = rigidbodyDrag;
        rigidbody.angularDrag = rigidbodyAngularDrag;
    }

    public void DropObject()
    {
        Destroy(configJoint);
        _DropObject();
    }

    private void OnJointBreak(float breakForce)
    {
        _DropObject();
    }

    private void _DropObject()
    {
        if (HasGrabbedObject)
        {
            cachedRigidbodyConfig.ApplyConfiguration(GrabbedObject.Physics.Rigidbody);
            GrabbedObject = null;
        }
    }
}