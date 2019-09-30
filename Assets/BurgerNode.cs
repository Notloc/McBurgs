using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerNode : MonoBehaviour
{
    [SerializeField] Collider[] nodeColliders;

    public void Enable()
    {
        foreach(Collider nodeCollider in nodeColliders)
            nodeCollider.enabled = true;
    }

    public void Disable()
    {
        foreach (Collider nodeCollider in nodeColliders)
            nodeCollider.enabled = false;
    }

    /// <summary>
    /// Checks if the node is above the BurgerComponent it is attached to
    /// </summary>
    /// <returns></returns>
    public bool IsAboveBurgerComponent()
    {
        float yPos = 0f;

        IBurgerComponent component = GetComponentInParent<IBurgerComponent>();
        if (InterfaceUtil.IsNull(component) == false)
            yPos = component.transform.position.y;

        if (this.transform.position.y > yPos)
            return true;

        return false;
    }
}
