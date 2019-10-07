using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerManager : MonoBehaviour
{
    public static CustomerManager Instance;

    [Header("Required References")]
    [SerializeField] Transform spawnPoint;
    [SerializeField] Transform exitPoint;
    [SerializeField] Customer customerPrefab;

    [Header("Options")]
    [SerializeField] int maxCustomers = 5;
    [SerializeField] Vector2 spawnDelayRange;

    private List<Customer> customers = new List<Customer>();
    private float spawnTimer;


    public Vector3 Exit { get { return exitPoint.position; } }

    private void Awake()
    {
        Instance = this;
    }

    private void FixedUpdate()
    {
        TrySpawnCustomer();
    }

    private void TrySpawnCustomer()
    {
        if (customers.Count >= maxCustomers || spawnTimer > Time.time)
            return;

        spawnTimer = Time.time + Random.Range(spawnDelayRange.x, spawnDelayRange.y);

        var customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
        customers.Add(customer);
    }


    private void OnTriggerEnter(Collider other)
    {
        Customer customer = other.GetComponentInParent<Customer>();
        if (customer)
        {
            customers.Remove(customer);
            Destroy(customer.gameObject);
        }
    }
}
