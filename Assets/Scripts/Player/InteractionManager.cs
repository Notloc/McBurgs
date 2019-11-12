using UnityEngine;
using UnityEngine.Networking;

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
    [SerializeField] float throwHoldTime = 0.7f;
    [SerializeField] float throwForce = 10f;
    [SerializeField] float pickupCalculationOffset = 0.1f;

    [Header("Rotation Options")]
    [SerializeField] float rotationSensitivity = 3f;
    [SerializeField] float itemPositionSmoothing = 2f;
    [SerializeField] float itemRotationSmoothing = 15f;
    [SerializeField] LayerMask raycastMask;
    [SerializeField] LayerMask interactionLayers; // Valid layers for interaction


    public IGrabbable HeldItem { get; private set; }
    public IInteractable TargetInteractable { get; private set; }
    public Collider TargetCollider { get; private set; }

    private bool isHoldingItem { get { return !HeldItem.IsNull(); } }
    IGrabbable storedItem = null;
    bool isUsingItem = false;

    private Vector3 holdOffset = Vector3.zero;

    [Client]
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

    [ClientCallback]
    private void Update()
    {
        if (!isLocalPlayer)
            return;

        UpdateTargets();
        IInteractable target = TargetInteractable;

        TakeInput();

        if (!target.IsNull() && target as IGrabbable == null)
        {
            HandleInteractable(target);
        }
        else
        {
            HandleHeldItems(target as IGrabbable);
        }
    }

    bool interactReady, interact, interactUp, use, swap, rotate;
    float interactPressTime;
    [Client]
    private void TakeInput()
    {
        interact = Input.GetButtonDown(ControlBindings.INTERACT);
        interactUp = Input.GetButtonUp(ControlBindings.INTERACT);

        if (interact)
        {
            interactReady = true;
            interactPressTime = Time.time;
        }

        use = Input.GetButton(ControlBindings.USE_ITEM);
        swap = Input.GetButtonDown(ControlBindings.SWAP_ITEM);
        rotate = Input.GetButton(ControlBindings.ROTATE_ITEM);


        float scrollDelta = Input.GetAxis(ControlBindings.HOLD_DISTANCE);
        holdOffset.z = Mathf.Clamp(holdOffset.z + scrollDelta, 0f, maxHoldOffset);


        if (rotate && HeldItem.IsNull() == false)
            controller.LookEnabled = false;
        else
            controller.LookEnabled = true;
    }

    private void HandleInteractable(IInteractable target)
    {
        bool interact = Input.GetButtonDown(ControlBindings.INTERACT);

        if (interact && !target.IsNull())
            CmdInteraction(target.netId);
    }

    [Command]
    private void CmdInteraction(NetworkInstanceId netId)
    {
        if (NetworkServer.objects.ContainsKey(netId) == false)
        {
            Debug.Log("Invalid netId passed to server.");
            return;
        }

        var obj = NetworkServer.objects[netId];
        var target = obj.GetComponent<IInteractable>();
        if (target.IsNull() == false)
        {
            target.Interact();
        }
    }

    [Client]
    private void HandleHeldItems(IGrabbable target)
    {
        // Swap item in hand with inv
        if (swap)
        {
            SwapItem();
            return;
        }

        // Pickup/Drop item
        if (!target.IsNull() && interact && interactReady)
        {
            if (!isHoldingItem)
            {
                interactReady = false;
                PickUpItem(target.netId);
            }
            return;
        }

        if (interactUp && interactReady)
        {
            if (isHoldingItem)
            {
                // If button was held long enough
                if (Time.time - interactPressTime >= throwHoldTime)
                    ThrowItem(HeldItem);
                else
                    DropItem(HeldItem);

                interactReady = false;
            }
        }

        IUsable usable = HeldItem as IUsable;
        if (usable != null)
        {
            // Use/Stop using held item
            if (use && isHoldingItem)
            {
                UseItem(usable);
            }
            else if (!use && isHoldingItem)
            {
                StopUsingItem(usable);
            }
        }

        if (rotate)
            RotateItem();
    }

    [Client]
    private void RotateItem()
    {
        if (HeldItem.IsNull())
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
    private void PickUpItem(NetworkInstanceId grabbableId)
    {
        IGrabbable target = ClientScene.objects[grabbableId].GetComponent<IGrabbable>();

        if (HeldItem != null || target == null || target.Locked)
            return;

        if (target.SnapToHands == false)
            holdOffset = CalculateGrabOffset(new Ray(heldItemParent.position, heldItemParent.forward),
                            (heldItemParent.position + (heldItemParent.rotation * target.GrabOffset)),
                             target.transform.position);
        else
            holdOffset = Vector3.zero;

        target.transform.SetParent(heldItemParent);

        if (target.GrabRotation.Equals(Quaternion.identity) == false)
            target.transform.localRotation = Quaternion.Euler(0f, target.transform.localRotation.eulerAngles.y, 0f);

        HeldItem = target;

        if (!isServer)
            CmdPickUpItem(grabbableId);
        else
            target.Lock();
    }

    [Command]
    private void CmdPickUpItem(NetworkInstanceId grabbableId)
    {
        IGrabbable target = NetworkServer.objects[grabbableId].GetComponent<IGrabbable>();

        if (target == null || target.Locked)
            return;

        target.Lock();
        target.transform.SetParent(heldItemParent);
        target.gameObject.GetComponent<NetworkIdentity>().AssignClientAuthority(connectionToClient);
    }

    private Vector3 CalculateGrabOffset(Ray line, Vector3 origin, Vector3 location)
    {
        int iterations = 30;

        float closestLength = 99f;
        int closestIndex = 0;

        float step = maxHoldOffset / (float)iterations;

        Vector3 calcOffset = pickupCalculationOffset * line.direction.normalized;
        for (int i=0; i<iterations; i++)
        {
            Vector3 offset = (line.direction.normalized * step * i) + calcOffset;

            Vector3 pos = origin + offset;

            float dist = (pos - location).sqrMagnitude;
            if (dist < closestLength)
            {
                closestIndex = i;
                closestLength = dist;
            }
        }

        return new Vector3(0f, 0f, step * closestIndex);
    }

    public void Drop(IGrabbable item)
    {
        if (HeldItem == item)
            DropItem(item);
    }
    private void DropItem(IGrabbable item)
    {
        if (item.IsNull())
            return;

        

        item.gameObject.transform.SetParent(null);

        if (item == HeldItem)
        {
            HeldItem = null;
            StopUsingItem(item as IUsable);
        }

        if (!isServer)
            CmdDropItem(item.netId);
        else
            item.Unlock();
    }
    [Command]
    private void CmdDropItem(NetworkInstanceId grabbableId)
    {
        IGrabbable item = NetworkServer.objects[grabbableId].GetComponent<IGrabbable>();
        if (item.IsNull())
            return;

        item.Unlock(); 
        item.gameObject.transform.SetParent(null);

        var netIdentity = item.gameObject.GetComponent<NetworkIdentity>();
        if (netIdentity.clientAuthorityOwner == connectionToClient)
            netIdentity.RemoveClientAuthority(connectionToClient);
    }
    
    private void ThrowItem(IGrabbable item)
    {
        if (item.IsNull())
            return;

        if (item == HeldItem)
            DropItem(item);

        CmdThrowItem(item.netId);
    }

    [Command]
    private void CmdThrowItem(NetworkInstanceId grabbableId)
    {
        IGrabbable item = NetworkServer.objects[grabbableId].GetComponent<IGrabbable>();
        if (item.IsNull())
            return;

        item.Rigidbody.AddForce(targetCamera.transform.forward * throwForce, ForceMode.Impulse);
    }


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
        if (item.IsNull())
            return null;

        storedItem = null;
        item.gameObject.SetActive(true);
        item.transform.localPosition = Vector3.down;
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
            PickUpItem(wasStored.netId);
        }
    }
    //


    [ClientCallback]
    private void LateUpdate()
    {
        if (!isLocalPlayer)
            return;
        PositionHeldItem();
    }

    // Drive the held objects position to keep it properly positioned
    // Held object is NOT kinematic, as it needs to interact with kinematic objects
    Vector3 heldItemTargetPosition = Vector3.zero;
    Quaternion heldItemTargetRotation = Quaternion.identity;
    private void PositionHeldItem()
    {
        if (HeldItem.IsNull() == false)
        {
            IUsable usable = HeldItem as IUsable;
            if (isUsingItem && !usable.IsNull())
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
