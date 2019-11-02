using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(IUsable))]
public class BurgerComponent : MonoBehaviour, IBurgerComponent
{
    [Header("Nodes")]
    [SerializeField] BurgerNode topNode;
    [SerializeField] BurgerNode bottomNode;

    [Header("Options")]
    [SerializeField] float componentSpacingMult = 0.5f;

    IUsable usable;

    public IFood Food => throw new System.NotImplementedException();

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

    private void DropSelf()
    {
        ClientController.Instance.Player.GetComponent<InteractionManager>().Drop(usable);
    }
}
