using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CookableObject : MonoBehaviour
{
    [SerializeField] float baseCookingTime = 30f;
    [SerializeField, Range(0f, 1f)] float cookedThreshold = 0.8f;
    [SerializeField, Range(0f, 1f)] float burntThreshold = 0.5f;

    public bool IsCooked { get { return percentCooked >= cookedThreshold; } }
    public bool IsBurnt { get { return percentBurnt >= burntThreshold; } }

    public UnityAction<float, float> OnCooking;
    
    private float percentCooked = 0f;
    private float percentBurnt = 0f;

    public void Cook(float cookingAmount)
    {
        cookingAmount = cookingAmount / baseCookingTime;
        percentCooked += cookingAmount;
        if (percentCooked > 1f)
        {
            float burningAmount = percentCooked - 1f;
            percentBurnt = Mathf.Clamp01(percentBurnt + burningAmount);
            percentCooked = 1f;
        }
        OnCooking?.Invoke(percentCooked, percentBurnt);
    }

    public CookState GetCookState()
    {
        if (IsBurnt)
            return CookState.BURNT;
        else if (IsCooked)
            return CookState.COOKED;
        else
            return CookState.RAW;
    }
}
