using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CustomerQueue : NetworkBehaviour
{
    [Header("Required References")]
    [SerializeField] Transform entrance;
    [SerializeField] Transform exit;

    [Header("Options")]
    [SerializeField] int maxAllowedInQueue;

    public bool IsFull { get { return maxAllowedInQueue <= queuedCustomers.Count; } }
    public bool IsEmpty { get { return queuedCustomers.Count == 0; } }

    public Transform Entrance { get { return entrance; } }
    public Transform Exit { get { return exit; } }

    private Queue<Customer> queuedCustomers = new Queue<Customer>();

    [Server]
    public bool Contains(Customer cust)
    {
        return queuedCustomers.Contains(cust);
    }

    [Server]
    public bool EnterQueue(Customer newCustomer)
    {
        if (!newCustomer || queuedCustomers.Count >= maxAllowedInQueue)
            return false;

        queuedCustomers.Enqueue(newCustomer);
        return true;
    }

    [Server]
    public Customer DequeueCustomer()
    {
        return queuedCustomers.Dequeue();
    }

    [ServerCallback]
    private void FixedUpdate()
    {
        UpdateStandingPositions();
    }

    [Server]
    private void UpdateStandingPositions()
    {
        float i = 0f;
        float count = maxAllowedInQueue;
        foreach(var customer in queuedCustomers)
        {
            Vector3 dest = Vector3.Lerp(exit.position, entrance.position, i/count);

            customer.Agent.SetDestination(dest);
            i++;
        }
    }

}
