using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillableOrderItem : SimpleOrderItem
{
    public float PercentFilled { get; protected set; }

    public FillableOrderItem(OrderItemType itemType, CookState cookState, float percentFilled) : base(itemType, cookState)
    {
        PercentFilled = percentFilled;
    }
}
