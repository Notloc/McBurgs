using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Till : MonoBehaviour, IInteractable
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
    public void Interact()
    {
        if (!waitingArea.IsFull && !customer)
            TakeOrder();
        else
            SubmitOrder();
    }

    private void TakeOrder()
    {
        customer = orderQueue.DequeueCustomer();
        customer.ShowOrder(orderDisplayTime);

        customer.agent.SetDestination(orderingPosition.position);

        ShowOrderTakingControls();
    }

    private void ShowOrderTakingControls()
    {

    }

    private void SubmitOrder()
    {
        waitingArea.EnterArea(customer);
        customer = null;
    }
}
