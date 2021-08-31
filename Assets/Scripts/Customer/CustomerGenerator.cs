using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerGenerator : MonoBehaviour
{
    [SerializeField] Customer customerPrefab = null;
    [SerializeField] Transform spawnPoint = null;

    public Customer CreateRandomCustomer()
    {
        Customer customer = Instantiate(customerPrefab, spawnPoint);


        customer.Style.Randomize();

        return customer;
    }


}
