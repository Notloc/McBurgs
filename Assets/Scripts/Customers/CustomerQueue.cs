using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerQueue : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Transform entrance;
    [SerializeField] Transform exit;

    [Header("Options")]
    [SerializeField] int maxAllowedInQueue;

    public bool IsFull { get { return maxAllowedInQueue <= queuedCustomers.Count; } }

    public Transform Entrance { get { return entrance; } }
    public Transform Exit { get { return exit; } }

    private Queue<Customer> queuedCustomers = new Queue<Customer>();

    public bool Contains(Customer cust)
    {
        return queuedCustomers.Contains(cust);
    }

    public bool EnterQueue(Customer newCustomer)
    {
        if (!newCustomer || queuedCustomers.Count >= maxAllowedInQueue)
            return false;

        queuedCustomers.Enqueue(newCustomer);
        return true;
    }

    public Customer DequeueCustomer()
    {
        return queuedCustomers.Dequeue();
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K))
            queuedCustomers.Dequeue();

        UpdateStandingPositions();
    }

    private void UpdateStandingPositions()
    {
        float i = 0f;
        float count = maxAllowedInQueue;
        foreach(var customer in queuedCustomers)
        {
            Vector3 dest = Vector3.Lerp(exit.position, entrance.position, i/count);

            customer.agent.SetDestination(dest);
            i++;
        }
    }

}
