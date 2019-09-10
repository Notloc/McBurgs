using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Items/New Item")]
public class ItemData : ScriptableObject
{
    [SerializeField] string itemName;
    [SerializeField] bool edible;
    [SerializeField] bool cookable;

    public string Name { get { return itemName; } }
    public bool IsEdible { get { return edible; } }
    public bool IsCookable { get { return cookable; } }

}
