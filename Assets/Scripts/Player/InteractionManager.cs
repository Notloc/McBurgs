using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manages a player's interactions with items and interactables in the game world.
/// </summary>
public class InteractionManager : PlayerComponent
{
    [Header("Required References")]
    [SerializeField] PlayerController controller;
    [SerializeField] Camera targetCamera;
    [SerializeField] Transform heldItemParent;

    [Header("Interaction Options")]
    [SerializeField] float interactionDistance = 1.5f;
    [SerializeField] float maxHoldOffset = 0.55f;
    [SerializeField] float rotationSensitivity = 3f;
    [SerializeField] float itemPositionSmoothing = 2f;
    [SerializeField] float itemRotationSmoothing = 15f;
    [SerializeField] LayerMask raycastMask;
    [SerializeField] LayerMask interactionLayers; // Valid layers for interaction


    public IGrabbable HeldItem { get; private set; }
    public IInteractable TargetInteractable { get; private set; }
    public Collider TargetCollider { get; private set; }

    private bool isHoldingItem { get { return HeldItem != null; } }
    IGrabbable storedItem = null;
    bool isUsingItem = false;

    private Vector3 holdOffset = Vector3.zero;

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

        TakeInput();

        if (target != null && InterfaceUtil.IsNull(target as IGrabbable))
        {
            HandleInteractable(target);
        }
        else
        {
            HandleGrabbable(target as IGrabbable);
        }
        
    }

    bool interact, use, swap, rotate;
    private void TakeInput()
    {
        interact = Input.GetButtonDown(ControlBindings.INTERACT);
        use = Input.GetButton(ControlBindings.USE_ITEM);
        swap = Input.GetButtonDown(ControlBindings.SWAP_ITEM);
        rotate = Input.GetButton(ControlBindings.ROTATE_ITEM);


        float scrollDelta = Input.GetAxis(ControlBindings.HOLD_DISTANCE);
        holdOffset.z = Mathf.Clamp(holdOffset.z + scrollDelta, 0f, maxHoldOffset);


        if (rotate && InterfaceUtil.IsNull(HeldItem) == false)
            controller.LookEnabled = false;
        else
            controller.LookEnabled = true;
    }

    private void HandleInteractable(IInteractable target)
    {
        bool interact = Input.GetButtonDown(ControlBindings.INTERACT);

        if (interact)
            target.Interact();
    }

    private void HandleGrabbable(IGrabbable target)
    {
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

        if (rotate)
            RotateItem();
    }

    private void RotateItem()
    {
        if (InterfaceUtil.IsNull(HeldItem))
            return;

        float xInput = -Input.GetAxis(ControlBindings.VIEW_INPUT_X) * rotationSensitivity;
        float yInput = Input.GetAxis(ControlBindings.VIEW_INPUT_Y) * rotationSensitivity;

        HeldItem.transform.localRotation = Quaternion.Euler(yInput, xInput, 0f) * HeldItem.transform.localRotation;
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
        if (item.ResetRotationAfterUse)
            item.transform.localRotation = item.GrabRotation;

        isUsingItem = false;
    }
    //


    // PICKUP/DROP FUNCTIONS
    //
    private void PickUpItem(IGrabbable target)
    {
        if (HeldItem != null || target == null || target.Locked)
            return;

        holdOffset.z = 0f;

        target.Lock();

        target.transform.SetParent(heldItemParent);
        target.transform.localPosition = target.GrabOffset;

        if (target.GrabRotation.Equals(Quaternion.identity) == false)
            target.transform.localRotation = Quaternion.Euler(0f, target.transform.localRotation.eulerAngles.y, 0f);

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
    Vector3 heldItemTargetPosition = Vector3.zero;
    Quaternion heldItemTargetRotation = Quaternion.identity;
    private void PositionHeldItem()
    {
        if (InterfaceUtil.IsNull(HeldItem) == false)
        {
            IUsable usable = HeldItem as IUsable;
            if (isUsingItem && !InterfaceUtil.IsNull(usable))
            {
                heldItemTargetPosition = usable.UseOffset;

                // Skip the position smoothing if desired
                if (usable.IgnorePositionSmoothing)
                    HeldItem.transform.localPosition = usable.UseOffset + holdOffset;

                if (usable.UseRotation.Equals(Quaternion.identity) == false)
                {
                    Quaternion localCameraRotation = Quaternion.Inverse(this.transform.rotation) * targetCamera.transform.rotation;
                    heldItemTargetRotation = localCameraRotation * usable.UseRotation;
                }
            }
            else
            {
                heldItemTargetPosition = HeldItem.GrabOffset;
                heldItemTargetRotation = HeldItem.transform.localRotation;
            }

            heldItemTargetPosition += holdOffset;

            HeldItem.transform.localPosition = Vector3.Lerp(HeldItem.transform.localPosition, heldItemTargetPosition, Time.deltaTime * itemPositionSmoothing);
            HeldItem.transform.localRotation = Quaternion.Slerp(HeldItem.transform.localRotation, heldItemTargetRotation, Time.deltaTime * itemRotationSmoothing);
        }
    }
}
