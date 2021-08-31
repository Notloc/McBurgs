using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CookableObject))]
public class CookingGraphics : MonoBehaviour
{
    [SerializeField] Renderer objectRenderer = null;
    [SerializeField] Color cookedColor = Color.green;
    [SerializeField] Color burntColor = Color.red;

    Color baseColor;

    private void Start()
    {
        baseColor = objectRenderer.material.color;
        GetComponent<CookableObject>().OnCooking += OnCooking;
    }

    private void OnCooking(float cookedPercent, float burntPercent)
    {
        Material mat = objectRenderer.material;

        Color targetColor = cookedPercent < 1f ? 
            Color.Lerp(baseColor, cookedColor, cookedPercent) : 
            Color.Lerp(cookedColor, burntColor, burntPercent);

        mat.color = targetColor;
    }
}
