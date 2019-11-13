using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(IUsable))]
public class BurgerComponent : NetworkBehaviour, IBurgerComponent
{
    [Header("Nodes")]
    [SerializeField] BurgerNode topNode;
    [SerializeField] BurgerNode bottomNode;

    [Header("Options")]
    [SerializeField] float componentSpacingMult = 0.62f;

    IUsable usable;

    public IFood Food => throw new System.NotImplementedException();

    [ClientCallback]
    private void Awake()
    {
        // Register for IUseable events to enable/disable build nodes
        usable = GetComponent<IUsable>();

        if (topNode)
        {
            usable.OnEnableEvent += topNode.Enable;
            usable.OnDisableEvent += topNode.Disable;
        }
        if (bottomNode)
        {
            usable.OnEnableEvent += bottomNode.Enable;
            usable.OnDisableEvent += bottomNode.Disable;
        }
    }

    [ClientCallback]
    private void OnTriggerEnter(Collider other)
    {
        var builder = other.GetComponentInParent<BurgerBuilder>();
        if (builder)
        {
            var closestNode = GetClosestNode(builder.ActiveNode);

            if (ValidateCollision(closestNode, builder.ActiveNode))
                CmdAttach(builder.netId, closestNode, this.transform.position, this.transform.rotation);
        }
    }

    // Returns the node closest to the given node
    private NodeLocation GetClosestNode(BurgerNode other)
    {
        if (!topNode)
            return NodeLocation.Bottom;
        if (!bottomNode)
            return NodeLocation.Top;

        float sqrDistTop = (topNode.transform.position - other.transform.position).sqrMagnitude;
        float sqrDistBottom = (bottomNode.transform.position - other.transform.position).sqrMagnitude;

        if (sqrDistTop > sqrDistBottom)
            return NodeLocation.Bottom;
        else
            return NodeLocation.Top;
    }

    public BurgerNode GetNode(NodeLocation loc)
    {
        if (loc == NodeLocation.Top)
            return topNode;
        else
            return bottomNode;
    }

    private bool ValidateCollision(NodeLocation node1Loc, BurgerNode node2)
    {
        BurgerNode node1 = node1Loc == NodeLocation.Top ? topNode : bottomNode;
        if (!node1 || !node2)
            return false;

        // Valid collision only if the nodes are not on the same side of the burger components
        bool isAbove = node1.IsAboveBurgerComponent();
        bool isAbove2 = node2.IsAboveBurgerComponent();

        return (isAbove != isAbove2);
    }


    [Command]
    private void CmdAttach(NetworkInstanceId builderId, NodeLocation nodeLoc, Vector3 position, Quaternion rotation)
    {
        var builder = NetworkServer.objects[builderId].GetComponent<BurgerBuilder>();
        var node = nodeLoc == NodeLocation.Top ? topNode : bottomNode;

        this.transform.position = position;
        this.transform.rotation = rotation;

        builder.Attach(node, nodeLoc);
    }

    public BurgerNode AttachTo(BurgerBuilder builder, BurgerNode targetNode, BurgerNode selfNode)
    {
        // selfNode must be ours
        if (selfNode != topNode && selfNode != bottomNode)
            return targetNode;

        DropSelf();
        usable.Rigidbody.constraints = RigidbodyConstraints.FreezeAll;

        selfNode.Disable();

        // Attach
        this.transform.SetParent(targetNode.transform);

        // Position the component nicely
        //
        // Cache localoffset without y
        Vector3 localOffset = this.transform.localPosition;
        localOffset.y = 0;
        // Reset local position
        this.transform.localPosition = Vector3.zero;
        // Move position based on node positions
        this.transform.position -= (selfNode.transform.position - targetNode.transform.position) * componentSpacingMult;
        // Reapply offset so the burger can be messy
        this.transform.localPosition += localOffset;
        //

        // Align rotation with builder and apply local spin
        Vector3 rot = this.transform.rotation.eulerAngles;

        this.transform.rotation = builder.transform.rotation;
        this.transform.localRotation *= Quaternion.Euler(0f, rot.y, 0f);

        ItemObject item = usable as ItemObject;
        if (item)
        {
            Destroy(item.Rigidbody);
            item.ChangeRigidbody(this.GetComponentInParent<Rigidbody>());
        }

        if (selfNode == topNode)
        {
            // Rotate 180, as we need to be upside down
            this.transform.localRotation *= Quaternion.Euler(180f, 0, 0);
            return bottomNode;
        }
        else
            return topNode;
    }
    
    [Client]
    private void DropSelf()
    {
        if (!isServer)
            ClientController.Instance.Player.GetComponent<InteractionManager>().Drop(usable);
    }
}
