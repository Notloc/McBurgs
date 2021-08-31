using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class JointConnectionSystem : MonoBehaviour
{
    [Header("Rigidbody Overrides")]
    [SerializeField] bool overrideRigidbodyMass = true;
    [SerializeField] float rigidbodyMass = 0.1f;
    [Space]
    [SerializeField] bool overrideRigidbodyDrag = true;
    [SerializeField] float rigidbodyDrag = 5f;
    [SerializeField] float rigidbodyAngularDrag = 2f;

    public UnityAction<GameObject> OnObjectDisconnected;

    private Dictionary<GameObject, Joint> jointsByObject = new Dictionary<GameObject, Joint>();
    private Dictionary<GameObject, RigidbodyConfiguration> rigidbodyConfigsByObject = new Dictionary<GameObject, RigidbodyConfiguration>();
    private List<GameObject> connectedGameObjects = new List<GameObject>();

    public void ConnectObject(JointConnectionComponent attachmentComponent, Vector3 connectionPoint)
    {
        GameObject target = attachmentComponent.gameObject;
        PhysicsComponent targetPhysics = attachmentComponent.Physics;
        if (connectedGameObjects.Contains(target) || targetPhysics.IsConnectedToJoint)
        {
            return;
        }

        Joint joint = CreateJoint();
        SetJointAnchorPoints(joint, attachmentComponent, connectionPoint);
        OverrideRigidbodySettings(targetPhysics.Rigidbody);
        joint.connectedBody = targetPhysics.Rigidbody;

        RegisterConnectedObject(target, joint, targetPhysics);
    }

    private void RegisterConnectedObject(GameObject targetObject, Joint joint, PhysicsComponent physics)
    {
        connectedGameObjects.Add(targetObject);
        rigidbodyConfigsByObject.Add(targetObject, new RigidbodyConfiguration(physics.Rigidbody));
        jointsByObject.Add(targetObject, joint);

        physics.SetConnectedToJoint(true);
    }

    protected virtual void SetJointAnchorPoints(Joint joint, JointConnectionComponent attachmentComponent, Vector3 connectionPoint)
    {
        Vector3 closestPoint = attachmentComponent.GetClosestConnectionPoint(connectionPoint);

        PhysicsComponent physics = attachmentComponent.Physics;
        joint.anchor = transform.InverseTransformPoint(connectionPoint);
        joint.connectedAnchor = physics.transform.InverseTransformPoint(closestPoint);
    }

    protected virtual void OverrideRigidbodySettings(Rigidbody rigidbody)
    {
        if (overrideRigidbodyMass)
        {
            rigidbody.mass = rigidbodyMass;
        }
        if (overrideRigidbodyDrag)
        {
            rigidbody.drag = rigidbodyDrag;
            rigidbody.angularDrag = rigidbodyAngularDrag;
        }
    }

    protected abstract Joint CreateJoint();

    private void OnJointBreak(float breakForce)
    {
        StartCoroutine(DetermineWhichJointBroke());
    }

    private IEnumerator DetermineWhichJointBroke()
    {
        yield return null; // Wait 1 frame

        foreach (GameObject connected in connectedGameObjects)
        {
            if (!jointsByObject[connected])
            {
                DisconnectObject(connected);
                yield break;
            }
        }
    }

    public void DisconnectObject(GameObject gameObject)
    {
        if (connectedGameObjects.Contains(gameObject))
        {
            UnregisterConnectedObject(gameObject);
            OnObjectDisconnected?.Invoke(gameObject);
        }
    }

    private void UnregisterConnectedObject(GameObject targetObject)
    {
        Destroy(jointsByObject[targetObject]);
        jointsByObject.Remove(targetObject);

        // Restore physics settings if object is not Destroyed
        if (targetObject)
        {
            PhysicsComponent physics = targetObject.GetComponent<PhysicsComponent>();
            physics.SetConnectedToJoint(false);
            rigidbodyConfigsByObject[targetObject].ApplyConfiguration(physics.Rigidbody);
        }
        rigidbodyConfigsByObject.Remove(targetObject);

        connectedGameObjects.Remove(targetObject);
    }
}
