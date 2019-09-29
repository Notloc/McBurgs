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

    
}
