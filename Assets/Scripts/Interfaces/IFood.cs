using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IFood : IGameObject
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