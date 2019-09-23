using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IUsable))]
public class BurgerComponent : MonoBehaviour, IBurgerComponent
{
    [Header("Nodes")]
    [SerializeField] BurgerNode node1;
    [SerializeField] BurgerNode node2;

    IUsable usable;

    private void Awake()
    {
        // Register for IUseable events to enable/disable build nodes
        usable = GetComponent<IUsable>();

        usable.OnEnableEvent += node1.Enable;
        usable.OnDisableEvent += node1.Disable;

        if (node2)
        {
            usable.OnEnableEvent += node2.Enable;
            usable.OnDisableEvent += node2.Disable;
        }
    }

    public BurgerNode AttachTo(BurgerNode targetNode)
    {
        DropSelf();
        usable.Rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        BurgerNode closestNode;

        if (!node2)
            closestNode = node1;
        else
        {
            float dist1 = Vector3.SqrMagnitude(targetNode.transform.position - node1.transform.position);
            float dist2 = Vector3.SqrMagnitude(targetNode.transform.position - node2.transform.position);

            closestNode = (dist1 < dist2) ? node1 : node2;
        }

        closestNode.Disable();

        // Attach
        this.transform.SetParent(targetNode.transform);

        // Offset so the component doesn't float but can be misaligned
        Vector3 localOffset = this.transform.localPosition;
        localOffset.y = 0;
        this.transform.position -= (closestNode.transform.position - targetNode.transform.position);
        this.transform.localPosition += localOffset;

        ItemObject item = usable as ItemObject;
        if (item)
        {
            Destroy(item.Rigidbody);
            item.ChangeRigidbody(this.GetComponentInParent<Rigidbody>());
        }

        if (closestNode == node1)
            return node2;
        else
            return node1;
    }

    private void DropSelf()
    {
        GameController.Instance.Player.GetComponent<InteractionManager>().Drop(usable);
    }
}
