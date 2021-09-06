using Codice.CM.Common.Serialization.Replication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PhysicsComponent))]
public class BurgerAttachableComponent : MonoBehaviour
{
    [SerializeField] BurgerIngredientType ingredientType = BurgerIngredientType.CHEESE;

    public GameObject GraphicsPrefab => graphicsPrefab;
    [SerializeField] GameObject graphicsPrefab = null;

    public float IngredientHeight => ingredientHeight;
    [SerializeField] float ingredientHeight = 0.05f;

    public PhysicsComponent Physics { get; private set; }
    public bool IsAttached { get; private set; }


    private void Awake()
    {
        Physics = GetComponent<PhysicsComponent>();
    }

    public BurgerIngredient GetIngredient()
    {
        return new BurgerIngredient(ingredientType);
    }

    public void SetAttached(bool isAttached)
    {
        IsAttached = isAttached;
    }

    private void OnValidate()
    {
        if (graphicsPrefab && transform.childCount == 0)
        {
            Instantiate(graphicsPrefab, transform);
        }
    }
}
