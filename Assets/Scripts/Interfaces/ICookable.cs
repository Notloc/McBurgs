using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum CookingType
{
    Grill,
    Fry
}

public interface ICookable : IGameObject
{
    float PercentCooked { get; }
    float PercentMiscooked { get; }

    void Cook(CookingType type, float deltaTime);
}
