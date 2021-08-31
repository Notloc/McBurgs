using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsComponent))]
[RequireComponent(typeof(CookableObject))]
public class CookableOrderItemComponent : MonoBehaviour, IOrderItemProvider
{
    [SerializeField] OrderItemType orderItemType = OrderItemType.NONE;

    public PhysicsComponent Physics { get; protected set; }

    private CookableObject cookable;

    private void Awake()
    {
        Physics = GetComponent<PhysicsComponent>();
        cookable = GetComponent<CookableObject>();
    }

    public OrderItem GetOrderItem()
    {   
        return new SimpleOrderItem(orderItemType, cookable.GetCookState());
    }
}
