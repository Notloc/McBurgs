using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform exitPoint;
    [SerializeField] Customer customerPrefab;

    [Header("Options")]
    [SerializeField] int maxCustomers = 5;
    [SerializeField] float spawnDelay = 7f;

    private List<Customer> customers = new List<Customer>();
    private float spawnTimer;


    private void FixedUpdate()
    {
        TrySpawnCustomer();
    }

    private void TrySpawnCustomer()
    {
        if (customers.Count >= maxCustomers || spawnTimer > Time.time)
            return;

        spawnTimer = Time.time + spawnDelay;

        var customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        customers.Add(customer);
    }
}
