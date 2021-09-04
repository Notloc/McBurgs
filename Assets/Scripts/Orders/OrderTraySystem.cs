using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsComponent))]
public class OrderTraySystem : MonoBehaviour
{
    [SerializeField] ConfigurableJointSettings orderItemJointSettings = new ConfigurableJointSettings();

    private List<IOrderItemProvider> orderItemProviders = new List<IOrderItemProvider>();
    private PhysicsComponent physics;

    private void Awake()
    {
        physics = GetComponent<PhysicsComponent>();
    }

    public List<OrderItem> GetOrderItems()
    {
        List<OrderItem> orderItems = new List<OrderItem>();

        foreach (IOrderItemProvider provider in orderItemProviders)
        {
            orderItems.Add(provider.GetOrderItem());
        }

        return orderItems;
    }

    private void OnCollisionEnter(Collision collision)
{
        IOrderItemProvider orderItem = collision.gameObject.GetComponentInParent<IOrderItemProvider>();
        if (orderItem == null || orderItem.Physics.IsLocked)
            return;

        AttachOrderItemToTray(orderItem, collision);
    }

    private void AttachOrderItemToTray(IOrderItemProvider provider, Collision collision)
    {
        if (physics.IsLocked || provider.Physics.IsLocked || orderItemProviders.Contains(provider))
            return;

        orderItemProviders.Add(provider);
        ConnectItemToTrayWithJoint(provider.Physics, collision);
    }

    private void ConnectItemToTrayWithJoint(PhysicsComponent objectWithPhysics, Collision collision)
    {
        ConfigurableJoint joint = gameObject.AddComponent<ConfigurableJoint>();
        orderItemJointSettings.Apply(joint, objectWithPhysics.Rigidbody.mass);

        Vector3 contactPoint = collision.GetContact(0).point;

        joint.anchor = transform.InverseTransformPoint(contactPoint);
        joint.connectedAnchor = objectWithPhysics.transform.InverseTransformPoint(contactPoint);

        joint.connectedBody = objectWithPhysics.Rigidbody;

        objectWithPhysics.RigidbodyOverrides.Interpolation.Set(RigidbodyInterpolation.Interpolate);
    }

    
}
