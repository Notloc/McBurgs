using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsComponent))]
public class BurgerSystem : MonoBehaviour, IOrderItemProvider
{
    [SerializeField] Transform attachmentTrigger = null;
    [SerializeField] float attachmentThreshold = 0.2f;

    public PhysicsComponent Physics { get; private set; }
    
    private List<BurgerIngredient> burgerIngredients = new List<BurgerIngredient>();

    private void Awake()
    {
        Physics = GetComponent<PhysicsComponent>();
    }

    public OrderItem GetOrderItem()
    {
        return new BurgerOrderItem(burgerIngredients);
    }


    private void OnTriggerEnter(Collider other)
    {
        BurgerAttachableComponent attachable = other.GetComponentInParent<BurgerAttachableComponent>();
        if (attachable && !attachable.IsAttached)
        {
            HandleAttachable(attachable);
        }
    }

    private void HandleAttachable(BurgerAttachableComponent attachable)
    {
        float distance = (attachable.transform.position - attachmentTrigger.position).magnitude;
        if (distance < attachmentThreshold)
        {
            Attach(attachable);
        }
    }

    private void Attach(BurgerAttachableComponent attachable)
    {
        burgerIngredients.Add(attachable.GetIngredient());

        GameObject graphics = Instantiate(attachable.GraphicsPrefab, transform);
        graphics.transform.position = attachmentTrigger.position;

        attachmentTrigger.localPosition = attachmentTrigger.localPosition + Vector3.up * attachable.IngredientHeight;

        Destroy(attachable.gameObject);
    }

}
