using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Till : NetworkBehaviour, IInteractable
{
    [Header("Required Reference")]
    [SerializeField] CustomerQueue orderQueue;
    [SerializeField] WaitingArea waitingArea;
    [SerializeField] Transform orderingPosition;

    [Header("Options")]
    [SerializeField] float orderDisplayTime = 6f;

    public CustomerQueue Line { get { return orderQueue; } }
    public WaitingArea WaitingArea { get { return waitingArea; } }

    private Customer customer;

    [Server]
    public void Interact()
    {
        if (!waitingArea.IsFull && !customer)
            TakeOrder();
        else
            SubmitOrder();
    }

    [Server]
    private void TakeOrder()
    {
        if (orderQueue.IsEmpty)
            return;

        customer = orderQueue.DequeueCustomer();
        customer.ShowOrder(orderDisplayTime);

        customer.Agent.SetDestination(orderingPosition.position);

        ShowOrderTakingControls();
    }

    [Server]
    private void ShowOrderTakingControls()
    {

    }

    [Server]
    private void SubmitOrder()
    {
        waitingArea.EnterArea(customer);
        customer = null;
    }
}
