using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtensions
{
    /// <summary>
    /// Sets the layer of the GameObject and all its children.
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="layer"></param>
    public static void SetLayerRecursively(this GameObject obj, LayerMask layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
            child.gameObject.SetLayerRecursively(layer);
    }

    /// <summary>
    /// Returns true if the LayerMask contains the layer indexed by the given int.
    /// </summary>
    /// <param name="self"></param>
    /// <param name="layer"></param>
    /// <returns></returns>
    public static bool Contains(this LayerMask self, int layer)
    {
        return ((self) & (1<<layer)) != 0;
    }

    public static bool IsNull(this IGameObject self)
    {
        if (self == null)
            return true;
        return !(self as MonoBehaviour);
    }
}
