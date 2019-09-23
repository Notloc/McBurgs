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
            AddNode(node);
    }

    private void AddNode(BurgerNode node)
    {
        var component = node.GetComponentInParent<IBurgerComponent>();
        if (InterfaceUtil.IsNull(component))
            return;

        activeNode.Disable();
        activeNode = component.AttachTo(activeNode);
    } 
}
