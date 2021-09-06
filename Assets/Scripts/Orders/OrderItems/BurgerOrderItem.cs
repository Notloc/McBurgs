using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerOrderItem : OrderItem
{
    public override OrderItemType ItemType { get => OrderItemType.BURGER; }

    public List<BurgerIngredient> BurgerContents { get; protected set; }

    public BurgerOrderItem(List<BurgerIngredient> burgerContents)
    {
        this.BurgerContents = burgerContents;
    }
}
