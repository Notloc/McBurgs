using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsComponent))]
public class BurgerSystem : MonoBehaviour, IOrderItemProvider
{
    [SerializeField] Collider attachmentTrigger = null;
    [SerializeField] float attachmentThreshold = 0.2f;

    public PhysicsComponent Physics { get; private set; }
    
    public List<BurgerIngredient> burgerIngredients = new List<BurgerIngredient>();

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
        float distance = (attachable.transform.position - attachmentTrigger.transform.position).magnitude;
        if (distance < attachmentThreshold)
        {
            Attach(attachable);
        }
    }

    private void Attach(BurgerAttachableComponent attachable)
    {
        BurgerIngredient ingredient = attachable.GetIngredient();
        burgerIngredients.Add(ingredient);

        GameObject graphics = Instantiate(attachable.GraphicsPrefab, transform);
        graphics.transform.position = attachmentTrigger.transform.position;

        attachmentTrigger.transform.localPosition = attachmentTrigger.transform.localPosition + Vector3.up * attachable.IngredientHeight;

        if (ingredient.IngredientType == BurgerIngredientType.BUN_TOP)
        {
            attachmentTrigger.enabled = false;
        }

        Destroy(attachable.gameObject);
    }

}
