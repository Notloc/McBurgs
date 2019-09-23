using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerBuilder : MonoBehaviour
{
    [SerializeField] BurgerNode startNode;
    BurgerNode activeNode;

    private void Awake()
    {
        activeNode = startNode;
    }

    private void OnTriggerEnter(Collider other)
    {
        var node = other.GetComponent<BurgerNode>();
        if (node)
            Attach(node);
    }

    private void Attach(BurgerNode node)
    {
        var component = node.GetComponentInParent<IBurgerComponent>();
        if (InterfaceUtil.IsNull(component))
            return;

        activeNode.Disable();
        activeNode = component.AttachTo(activeNode);

        if (activeNode)
            activeNode.Enable();
        else
            FinishBurger();
    } 

    private void FinishBurger()
    {

    }
}
