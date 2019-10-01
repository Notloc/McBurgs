using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerBuilder : MonoBehaviour
{
    private static string BURGER_BUILDER_TAG = "BurgerBuilder";

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

    BurgerNode activeNode;
    BurgerNode previousNode;
    List<IBurgerComponent> burgerComponents;

    private void Awake()
    {
        burgerComponents = new List<IBurgerComponent>();
        activeNode = startNode;
    }

    private void OnTriggerEnter(Collider other)
    {
        string tag = other.tag;
        if (tag.Equals(BURGER_BUILDER_TAG))
            return;

        var node = other.GetComponent<BurgerNode>();
        if (node)
            if (ValidateCollision(node))
                Attach(node);
    }

    private bool ValidateCollision(BurgerNode node)
    {
        if (!activeNode || !node)
            return false;

        // Valid collision only if the nodes are not on the same side of the burger components
        bool activeAbove = activeNode.IsAboveBurgerComponent();
        bool newAbove = node.IsAboveBurgerComponent();

        return (activeAbove != newAbove);
    }

    private void Attach(BurgerNode node)
    {
        var component = node.GetComponentInParent<IBurgerComponent>();
        if (InterfaceUtil.IsNull(component))
            return;

        previousNode = activeNode;
        previousNode.Disable();

        activeNode = component.AttachTo(activeNode, node);

        burgerComponents.Add(component);
        StartCoroutine(AttachAnimation());
    } 

    private IEnumerator AttachAnimation()
    {
        if (activeNode)
            activeNode.Disable();

        int i = 0;

        // Cache all the original offset values
        float[] originalOffsets = new float[burgerComponents.Count];
        foreach (IBurgerComponent component in burgerComponents)
        {
            originalOffsets[i] = component.transform.localPosition.y;
            i++;
        }

        // Animate over time
        float startTime = Time.time;
        bool validatedNewComponent = false, result = false;
        while (startTime + animationLength > Time.time)
        {
            float progress = (Time.time - startTime) / animationLength;

            i = 0;
            foreach (IBurgerComponent component in burgerComponents)
            {
                SetLocalYOffset(component.transform, progress, originalOffsets[i]);
                i++;
            }

            // Half way through the animation, Check if the newest piece should fall off
            if (progress > 0.5f && !validatedNewComponent)
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
            SetLocalYOffset(component.transform, 1f, originalOffsets[i]);
            i++;
        }

        if (activeNode)
        {
            activeNode.gameObject.tag = BURGER_BUILDER_TAG;
            activeNode.Enable();
        }
        else
            FinishBurger();
    }
    private void SetLocalYOffset(Transform t, float progress, float originalOffset)
    {
        Vector3 pos = t.localPosition;
        pos.y = originalOffset - (animationStrength * Mathf.Sin(progress * Mathf.PI));
        t.localPosition = pos;
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
        if (InterfaceUtil.IsNull(grabbable) == false)
            grabbable.ChangeRigidbody(r);

        burgerComponents.Remove(newest);

        activeNode = previousNode;
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
