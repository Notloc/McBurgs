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
    [SerializeField] Transform droppedItemPoint;

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
        if(Physics.Raycast(targetCamera.transform.position, targetCamera.transform.forward, out hit, interactionDistance, raycastMask))
        {
            int colliderLayer = hit.collider.gameObject.layer;

            if (interactionLayers.Contains(colliderLayer))
                TargetInteractable = hit.collider.GetComponentInParent<IInteractable>();
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

        if (HeldItem != null || target as IGrabbable != null)
        {
            HandleGrabbable(target as IGrabbable);
        }
        else if (target != null)
        {
            HandleInteractable(target);
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
        isUsingItem = true;
    }

    private void StopUsingItem(IUsable item)
    {
        if (!isUsingItem || item == null)
            return;

        item.DisableUse();
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
        target.gameObject.transform.localPosition = Vector3.zero;
        target.gameObject.transform.localRotation = Quaternion.identity;

        HeldItem = target;
    }
    private void DropItem(IGrabbable item)
    {
        if (item == null)
            return;

        item.Unlock();

        item.gameObject.transform.SetParent(null);
        item.gameObject.transform.position = droppedItemPoint.position;

        if (item == HeldItem)
        {
            StopUsingItem(item as IUsable);
            HeldItem = null;
        }
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

    private void UnstoreItem(IGrabbable item)
    {
        item.gameObject.SetActive(true);
        DropItem(item);
    }

    private void SwapItem()
    {
        IGrabbable wasStored = null;
        if (storedItem != null)
        {
            wasStored = storedItem;
            UnstoreItem(wasStored);
        }

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
}
