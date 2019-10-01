using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFood
{
    FoodType FoodType { get; }

}

public enum FoodType
{
    None,
    Bread,
    Meat,
    Vegetable,
    Cheese,
    Sauce,
    Inedible,
}