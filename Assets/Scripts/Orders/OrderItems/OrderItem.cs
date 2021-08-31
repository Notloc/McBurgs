using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public abstract class OrderItem
{
    public abstract OrderItemType ItemType { get; }
}
