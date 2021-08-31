using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Order
{
    List<OrderItem> orderItems = new List<OrderItem>();

    public Order() {}

    public Order(List<OrderItem> orderItems)
    {
        this.orderItems = orderItems;
    }

    public IList<OrderItem> GetOrderItems()
    {
        return orderItems.AsReadOnly();
    }
}
