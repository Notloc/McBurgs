using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CustomerStyle))]
public class Customer : MonoBehaviour
{
    public Order Order { get; private set; }
    public CustomerStyle Style { get; private set; }

    private void Awake()
    {
        Style = GetComponent<CustomerStyle>();
    }


}
