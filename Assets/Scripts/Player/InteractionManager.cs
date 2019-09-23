using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a player's interactions with items and interactables in the game world.
/// </summary>
public class InteractionManager : PlayerComponent
{
    [Header("Required References")]
    [SerializeField] Camera targetCamera;
    [SerializeField] Transform heldItemParent;

    [Header("Interaction Options")]
    [SerializeField] float interactionDistance = 1.5f;
    [SerializeField] LayerMask raycastMask;
    [SerializeField] LayerMask interactionLayers; // Valid layers for interaction


    public IGrabbable HeldItem { get; private set; }
    public IInteractable TargetInteractable { get; private set; }
    public Collider TargetCollider { get; private set; }

    private bool isHoldingItem { get { return HeldItem != null; } }
    IGrabbable storedItem = null;
    bool isUsingItem = false;

    private void UpdateTargets()
    {
        RaycastHit hit;
        if(Physics.Raycast(
            targetCamera.transform.position,
            targetCamera.transform.forward,
            out hit,
            interactionDistance,
            raycastMask,
            QueryTriggerInteraction.Ignore))
        {
            int colliderLayer = hit.collider.gameObject.layer;

            if (interactionLayers.Contains(colliderLayer))
            {
                var builder = hit.collider.GetComponentInParent<BurgerBuilder>();
                if (builder)
                    TargetInteractable = builder.GetComponent<IInteractable>();
                else
                    TargetInteractable = hit.collider.GetComponentInParent<IInteractable>();
            }
            else
                TargetInteractable = null;

            TargetCollider = hit.collider;
        }
        else
        {
            TargetCollider = null;
            TargetInteractable = null;
        }
    }

    private void Update()
    {
        UpdateTargets();
        IInteractable target = TargetInteractable;

        if (target != null && target as IGrabbable == null)
        {
            HandleInteractable(target);
        }
        else
        {
            HandleGrabbable(target as IGrabbable);
        }
        
    }

    private void HandleInteractable(IInteractable target)
    {
        bool interact = Input.GetButtonDown(ControlBindings.INTERACT);

        if (interact)
            target.Interact();
    }

    private void HandleGrabbable(IGrabbable target)
    {
        bool interact = Input.GetButtonDown(ControlBindings.INTERACT);
        bool use = Input.GetButton(ControlBindings.USE_ITEM);
        bool swap = Input.GetButtonDown(ControlBindings.SWAP_ITEM);

        // Swap item in hand with inv
        if (swap)
        {
            SwapItem();
            return;
        }

        // Pickup/Drop item
        if (interact)
        {
            if (!isHoldingItem)
                PickUpItem(target);
            else
                DropItem(HeldItem);

            return;
        }

        if (HeldItem as IUsable != null)
        {
            // Use/Stop using held item
            if (use && isHoldingItem)
            {
                UseItem(HeldItem as IUsable);
            }
            else if (!use && isHoldingItem)
            {
                StopUsingItem(HeldItem as IUsable);
            }
        }
    }

    // USE FUNCTIONS
    //
    private void UseItem(IUsable item)
    {
        if (isUsingItem || item == null)
            return;

        item.EnableUse();
        item.transform.localPosition = item.UseOffset;
        isUsingItem = true;
    }

    private void StopUsingItem(IUsable item)
    {
        if (!isUsingItem || item == null)
            return;

        item.DisableUse();

        if (HeldItem == item as IGrabbable)
            item.transform.localPosition = item.GrabOffset;

        isUsingItem = false;
    }
    //


    // PICKUP/DROP FUNCTIONS
    //
    private void PickUpItem(IGrabbable target)
    {
        if (HeldItem != null || target == null || target.Locked)
            return;

        target.Lock();

        target.gameObject.transform.SetParent(heldItemParent);
        target.gameObject.transform.localPosition = target.GrabOffset;
        target.gameObject.transform.localRotation = Quaternion.identity;

        HeldItem = target;
    }
    private void DropItem(IGrabbable item)
    {
        if (item == null)
            return;

        item.Unlock();

        item.gameObject.transform.SetParent(null);

        if (item == HeldItem)
        {
            HeldItem = null;
            StopUsingItem(item as IUsable);
        }
    }
    public void Drop(IGrabbable grabbable)
    {
        DropItem(grabbable);
    }
    //

    // INV FUNCTIONS
    //
    private void StoreItem(IGrabbable item)
    {
        item.gameObject.SetActive(false);
        item.gameObject.transform.SetParent(heldItemParent);

        storedItem = item;
    }

    private IGrabbable UnstoreItem()
    {
        IGrabbable item = storedItem;
        if (item == null)
            return null;

        item.gameObject.SetActive(true);
        return item;
    }

    private void SwapItem()
    {
        IGrabbable wasStored = UnstoreItem();

        if (HeldItem != null)
        {
            IGrabbable item = HeldItem;
            DropItem(item);
            StoreItem(item);
        }

        if (wasStored != null)
        {
            PickUpItem(wasStored);
        }
    }
    //



    private void LateUpdate()
    {
        PositionHeldItem();
    }

    // Drive the held objects position to keep it properly positioned
    // Held object is NOT kinematic, as it needs to interact with kinematic objects
    private void PositionHeldItem()
    {
        if (InterfaceUtil.IsNull(HeldItem) == false)
        {
            IUsable usable = HeldItem as IUsable;
            if (isUsingItem && !InterfaceUtil.IsNull(usable))
            {
                HeldItem.transform.localPosition = usable.UseOffset;
                HeldItem.transform.rotation = Quaternion.Euler(0f, this.transform.rotation.eulerAngles.y, 0f) * usable.UseRotation;
            }
            else
            {
                HeldItem.transform.localPosition = HeldItem.GrabOffset;
                HeldItem.transform.localRotation = Quaternion.identity;
            }
        }
    }
}
