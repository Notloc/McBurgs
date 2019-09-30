using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroySelf : MonoBehaviour
{
    [SerializeField] float delay = 5f;

    private void Awake()
    {
        Destroy(this.gameObject, delay);
    }
}
