using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerStyle : MonoBehaviour
{
    [SerializeField] Renderer customerRenderer = null;

    public void Randomize()
    {
        customerRenderer.material.color = Random.ColorHSV();
    }
}
