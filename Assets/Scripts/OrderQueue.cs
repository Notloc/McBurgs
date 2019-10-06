using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderQueue : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Transform entrance;
    [SerializeField] Transform exit;

    [Header("Options")]
    [SerializeField] int maxAllowedInQueue;

    public Transform Entrance { get { return entrance; } }
    public Transform Exit { get { return exit; } }

    private Queue<Customer> queuedCustomers = new Queue<Customer>();

    public bool EnterQueue(Customer newCustomer)
    {
        if (queuedCustomers.Count >= maxAllowedInQueue)
            return false;

        queuedCustomers.Enqueue(newCustomer);
        return true;
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyDown(KeyCode.K))
            queuedCustomers.Dequeue();
        UpdateQueuePositions();
    }

    private void UpdateQueuePositions()
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
