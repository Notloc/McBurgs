using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a player's interactions with items and interactables in the game world.
/// </summary>
public class InteractionManager : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Camera targetCamera;
    [SerializeField] Transform heldItemParent;
    [SerializeField] Transform droppedItemPoint;

    [Header("Options")]
    [SerializeField] float grabDistance = 1.5f;
    [SerializeField] LayerMask grabbableMask;

    private bool isHoldingItem { get { return heldItem != null; } }

    IGrabbable heldItem = null;
    IGrabbable storedItem = null;
    bool isUsingItem = false;

    private IInteractable GetTargetInteractable()
    {
        RaycastHit hit;
        if(Physics.Raycast(targetCamera.transform.position, targetCamera.transform.forward, out hit, grabDistance, grabbableMask))
        {
            return hit.collider.GetComponentInParent<IInteractable>();
        }
        return null;
    }

    private void Update()
    {
        IInteractable target = GetTargetInteractable();

        if (heldItem != null || target as IGrabbable != null)
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
                DropItem(heldItem);

            return;
        }

        if (heldItem as IUsable != null)
        {
            // Use/Stop using held item
            if (use && isHoldingItem)
            {
                UseItem(heldItem as IUsable);
            }
            else if (!use && isHoldingItem)
            {
                StopUsingItem(heldItem as IUsable);
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
        if (heldItem != null || target == null || target.Locked)
            return;

        target.Lock();

        target.gameObject.transform.SetParent(heldItemParent);
        target.gameObject.transform.localPosition = Vector3.zero;
        target.gameObject.transform.localRotation = Quaternion.identity;

        heldItem = target;
    }
    private void DropItem(IGrabbable item)
    {
        if (item == null)
            return;

        item.Unlock();

        item.gameObject.transform.SetParent(null);
        item.gameObject.transform.position = droppedItemPoint.position;

        if (item == heldItem)
        {
            StopUsingItem(item as IUsable);
            heldItem = null;
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

        if (heldItem != null)
        {
            IGrabbable item = heldItem;
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
