using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class BurgerBuilder : NetworkBehaviour
{
    [Header("Required Reference")]
    [SerializeField] BurgerNode startNode;
    [SerializeField] BurgerScorer scorer;
    [SerializeField] BurgerTooltipManager tooltip;

    [Header("Options")]
    [SerializeField] [Range(0f, 0.1f)] float attachOffsetLimit = 0.03f;
    [SerializeField] [Range(0f, 0.2f)] float centerOfMassRadiusLimit = 0.15f;
    [SerializeField] float detachmentForce = 5f;
    [Space]
    [SerializeField] [Range(0f,0.1f)] float animationStrength = 0.01f;
    [SerializeField] float animationLength = 0.4f;


    public BurgerNode ActiveNode { get; private set; }
    BurgerNode previousNode;
    List<IBurgerComponent> burgerComponents;

    private void Awake()
    {
        burgerComponents = new List<IBurgerComponent>();
        ActiveNode = startNode;
    }

    [Server]
    public void Attach(BurgerNode node, NodeLocation nodeLoc)
    {
        var component = node.GetComponentInParent<IBurgerComponent>();
        if (component.IsNull())
            return;

        previousNode = ActiveNode;
        previousNode.Disable();

        ActiveNode = component.AttachTo(this, ActiveNode, node);

        burgerComponents.Add(component);
        StartCoroutine(AttachAnimation());

        if (!isClient)
            RpcAttach(component.netId, nodeLoc);
    } 

    [ClientRpc]
    private void RpcAttach(NetworkInstanceId componentId, NodeLocation nodeLoc)
    {
        var component = ClientScene.objects[componentId].GetComponent<IBurgerComponent>();
        if (component.IsNull())
            return;

        var node = component.GetNode(nodeLoc);

        previousNode = ActiveNode;
        previousNode.Disable();

        ActiveNode = component.AttachTo(this, ActiveNode, node);

        burgerComponents.Add(component);
        StartCoroutine(AttachAnimation());
    }

    private IEnumerator AttachAnimation()
    {
        if (ActiveNode)
            ActiveNode.Disable();

        // Client animation and detachment
        if (isClient)
        {
            int i = 0;

            // Cache all the original position values
            Vector3[] originalOffsets = new Vector3[burgerComponents.Count];
            foreach (IBurgerComponent component in burgerComponents)
            {
                originalOffsets[i] = component.transform.localPosition;
                i++;
            }
        
            // Animate over time
            float startTime = Time.time;
            bool validatedNewComponent = false, result = false;
            while (startTime + animationLength > Time.time)
            {
                float progress = (Time.time - startTime) / animationLength;
                Vector3 up = this.transform.up;

                i = 0;
                foreach (IBurgerComponent component in burgerComponents)
                {
                    SetLocalYOffset(component.transform, progress, originalOffsets[i], up);
                    i++;
                }

                // Half way through the animation, Check if the newest piece should fall off
                if (!validatedNewComponent && progress > 0.5f)
                {
                    result = ValidateNewComponent();
                    if (!result)
                        DetachNewComponent();

                    validatedNewComponent = true;
                }

                yield return null;
            }
            
            // Restore offsets
            i = 0;
            foreach (IBurgerComponent component in burgerComponents)
            {
                SetLocalYOffset(component.transform, 1f, originalOffsets[i], Vector3.zero);
                i++;
            }
        }
        // Server detachment
        else
        {
            if (!ValidateNewComponent())
                DetachNewComponent();
        }

        if (ActiveNode)
        {
            ActiveNode.Enable();
        }
        else
            FinishBurger();
    }
    private void SetLocalYOffset(Transform component, float progress, Vector3 originalPos, Vector3 relativeUp)
    {
        component.localPosition = originalPos;
        component.position -= relativeUp * (animationStrength * Mathf.Sin(progress * Mathf.PI));
    }


    private void FinishBurger()
    {
        scorer.ScoreBurger(burgerComponents);
        tooltip.OnBurgerFinish();
    }





    // BURGER VALIDATION CODE
    //
    private bool ValidateNewComponent()
    {
        IBurgerComponent newest = burgerComponents[burgerComponents.Count - 1];
        float scale = this.transform.localScale.y;

        Vector3 newOffset = CalculateCenterOfComponents();
        newOffset.y = 0;

        if (newOffset.sqrMagnitude > Mathf.Pow(centerOfMassRadiusLimit * scale, 2))
            return false;

        if (Vector3.Distance(newest.transform.position, previousNode.transform.position) > attachOffsetLimit * scale)
            return false;

        return true;
    }

    private void DetachNewComponent()
    {
        IBurgerComponent newest = burgerComponents[burgerComponents.Count - 1];

        newest.transform.SetParent(null);
        newest.gameObject.layer = LayerManager.InteractionLayer;
        Rigidbody r = newest.gameObject.AddComponent<Rigidbody>();
        r.AddForce((newest.transform.position - this.transform.position).normalized * detachmentForce);

        IGrabbable grabbable = newest.gameObject.GetComponent<IGrabbable>();
        if (grabbable.IsNull() == false)
            grabbable.ChangeRigidbody(r);

        burgerComponents.Remove(newest);

        ActiveNode = previousNode;
    }

    private Vector2 CalculateCenterOfComponents()
    {
        Vector3 basePosition = this.transform.position;

        Vector3 center = Vector3.zero;
        foreach(IBurgerComponent component in burgerComponents)
            center += component.transform.position - basePosition;

        return Quaternion.Inverse(this.transform.rotation) * (center / (float)burgerComponents.Count);
    }

    //
}
