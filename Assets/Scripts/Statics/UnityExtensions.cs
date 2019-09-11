using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UnityExtensions
{
    public static void SetLayerRecursively(this GameObject obj, LayerMask layer)
    {
        obj.layer = layer;

        foreach (Transform child in obj.transform)
            child.gameObject.SetLayerRecursively(layer);
    }
}
