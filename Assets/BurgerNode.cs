using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerNode : MonoBehaviour
{
    [SerializeField] Collider nodeCollider;

    public void Enable()
    {
        nodeCollider.enabled = true;
    }

    public void Disable()
    {
        nodeCollider.enabled = false;
    }

    
}
