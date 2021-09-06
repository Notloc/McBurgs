using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerIngredient
{
    public BurgerIngredientType IngredientType { get; private set; }

    public BurgerIngredient(BurgerIngredientType ingredientType)
    {
        this.IngredientType = ingredientType;
    }
}
