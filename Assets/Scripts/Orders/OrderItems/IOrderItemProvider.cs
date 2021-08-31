using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IOrderItemProvider : IMonoBehaviour
{
    OrderItem GetOrderItem();
    PhysicsComponent Physics { get; }
}
