using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleOrderItem : OrderItem
{
    public override OrderItemType ItemType { get => itemType; }
    protected OrderItemType itemType;

    public CookState OrderItemState { get; protected set; }

    public SimpleOrderItem(OrderItemType itemType, CookState orderItemState)
    {
        this.itemType = itemType;
        this.OrderItemState = orderItemState;
    }
}
