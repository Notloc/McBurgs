using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerOrderItem : OrderItem
{
    public override OrderItemType ItemType { get => OrderItemType.BURGER; }

    public BurgerContents BurgerContents { get; protected set; }

    public BurgerOrderItem(BurgerContents burgerContents)
    {
        this.BurgerContents = burgerContents;
    }
}
