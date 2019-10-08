using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaitingArea : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Transform entrance;
    [SerializeField] Transform exit;

    [Header("Options")]
    [SerializeField] int maxAllowedInArea;

    public bool IsFull { get { return maxAllowedInArea <= waitingCustomers.Count; } }

    public Transform Entrance { get { return entrance; } }
    public Transform Exit { get { return exit; } }

    private List<Customer> waitingCustomers = new List<Customer>();

    public bool Contains(Customer cust)
    {
        return waitingCustomers.Contains(cust);
    }

    public bool EnterArea(Customer newCustomer)
    {
        if (!newCustomer || waitingCustomers.Count >= maxAllowedInArea)
            return false;

        waitingCustomers.Add(newCustomer);
        return true;
    }

    public void Remove(Customer cust)
    {
        if (waitingCustomers.Contains(cust))
            waitingCustomers.Remove(cust);
    }

    private void FixedUpdate()
    {
        UpdateStandingPositions();
    }

    private void UpdateStandingPositions()
    {
        float i = 0f;
        float count = maxAllowedInArea;
        foreach (var customer in waitingCustomers)
        {
            Vector3 dest = Vector3.Lerp(exit.position, entrance.position, i / count);

            customer.Agent.SetDestination(dest);
            i++;
        }
    }
}
