using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.AI;

public enum CustomerState
{
    Entering,
    Queuing,
    Ordering,
    Eating,
    Leaving
}

public class Customer : NetworkBehaviour
{
    [Header("Required Refereneces")]
    [SerializeField] GameObject moneyPrefab;
    [SerializeField] NavMeshAgent agent;

    public NavMeshAgent Agent { get { return agent; } }


    [SyncVar] private CustomerState state;
    private int subState;

    private FoodObject burger;

    public override void OnStartServer()
    {
        base.OnStartServer();
        SetState(CustomerState.Entering);
    }

    private void SetState(CustomerState newState)
    {
        state = newState;
        subState = 0;
    }

    private void FixedUpdate()
    {
        if (isServer)
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
                HandleQueuing();
                break;

            case CustomerState.Ordering:
                HandleOrdering();
                break;

            case CustomerState.Eating:
                HandleEating();
                break;
        }
    }

    // Sub States:
    //  0 - Standing
    //  1 - Heading to queue
    //  2 - At queue entrance
    CustomerQueue queue;
    WaitingArea waiting;
    private void HandleEntering()
    {
        if (subState == 0)
        {
            // Find the ordering queue
            var obj = GameObject.FindGameObjectWithTag(TagManager.Till);
            var til = obj.GetComponent<Till>();

            queue = til.Line;
            waiting = til.WaitingArea;

            // Set a path to it
            Agent.SetDestination(queue.Entrance.position);
            subState = 1;
        }

        if (subState == 1)
        {
            // Advance state once close enough
            if ((Agent.destination - Agent.transform.position).sqrMagnitude < 1.5f)
                subState = 2;
        }

        if (subState == 2)
        {
            bool result = queue.EnterQueue(this);
            if (result)
                SetState(CustomerState.Queuing);
        }
    }

    private void HandleQueuing()
    {  }


    float orderTimer = 0;
    private void HandleOrdering()
    {
        if (Time.time > orderTimer)
            ;// orderCanvas.gameObject.SetActive(false);

        if (orderReceived)
            SetState(CustomerState.Eating);
    }

    // Substates:
    //  0 - Just got food
    //  1 - Paid and leaving
    private void HandleEating()
    {
        if (subState == 0)
        {
            if (waiting.Contains(this))
                waiting.Remove(this);
            Agent.SetDestination(CustomerManager.Instance.Exit);

            PayForFood();

            subState = 1;
        }

    }

    private void PayForFood()
    {
        int cash = Random.Range(1, 12);
        for (int i=0; i<cash; i++)
            Instantiate(moneyPrefab, this.transform.position + (Vector3.up * 2), Quaternion.identity);
    }

    public void ShowOrder(float length)
    {
        orderTimer = Time.time + length;
        SetState(CustomerState.Ordering);

        // orderCanvas.SetOrder(order);
        // orderCanvas.gameObject.SetActive(true);
    }

    bool orderReceived;
    private void OnCollisionEnter(Collision collision)
    {
        if (state != CustomerState.Ordering)
            return;

        var burg = collision.collider.GetComponentInParent<BurgerBuilder>();
        if (!burg)
            return;

        orderReceived = true;
        burger = burg.GetComponent<FoodObject>();
        GameController.Instance.Player.GetComponent<InteractionManager>().Drop(burger);

        burger.Rigidbody.isKinematic = true;
        burger.transform.SetParent(this.transform);
        burger.transform.localPosition = Vector3.up * 2f;
        burger.transform.localRotation = Quaternion.identity;
    }
}
