using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public enum CustomerState
{
    Entering,
    Queuing,
    Ordering,
    Waiting,
    Eating,
    Leaving
}

public class Customer : MonoBehaviour
{
    public NavMeshAgent agent;

    private CustomerState state;
    private int subState;

    private void Awake()
    {
        SetState(CustomerState.Entering);
    }

    private void SetState(CustomerState newState)
    {
        state = newState;
        subState = 0;
    }

    private void FixedUpdate()
    {
        ProcessAi();
    }


    private void ProcessAi()
    {
        switch (state)
        {
            case CustomerState.Entering:
                HandleEntering();
                break;

            case CustomerState.Queuing:
                // Queueing AI is driven by the queue itself
                break;
        }
    }

    // Sub States:
    //  0 - Standing
    //  1 - Heading to queue
    //  2 - At queue entrance
    OrderQueue queue;
    private void HandleEntering()
    {
        if (subState == 0)
        {
            // Find the ordering queue
            var obj = GameObject.FindGameObjectWithTag(TagManager.OrderQueue);
            queue = obj.GetComponent<OrderQueue>();

            // Set a path to it
            agent.SetDestination(queue.Entrance.position);
            subState = 1;
        }

        if (subState == 1)
        {
            // Advance state once close enough
            if ((agent.destination - agent.transform.position).sqrMagnitude < 1.5f)
                subState = 2;
        }

        if (subState == 2)
        {
            bool result = queue.EnterQueue(this);
            if (result)
                SetState(CustomerState.Queuing);
        }
    }
}
