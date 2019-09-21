using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodObject : ItemObject, ICookable, IHaveTooltip
{
    [SerializeField] Renderer renderer;

    [Header("Cooking Options")]
    [SerializeField] CookingType cookingMethod;
    [SerializeField] [Range(0.01f, 5f)] float cookingRate = 0.05f;
    [SerializeField] Color cookedColor = Color.blue;
    [SerializeField] Color burntColor = Color.black;

    [Header("Tooltip")]
    [SerializeField] TooltipData tooltipData;
    [SerializeField] Vector3 displayOffset;

    public TooltipData TooltipData { get { return tooltipData; } }
    public Vector3 DisplayOffset { get {
            Vector3 rot = this.transform.rotation.eulerAngles;
            rot.x = 0;
            rot.z = 0;
            return Quaternion.Euler(rot) * displayOffset;
        } }

    Color originalColor;
    float percentCooked = 0f;
    float percentMiscooked = 0f;

    public float PercentCooked { get { return percentCooked;} }
    public float PercentMiscooked { get { return percentMiscooked; } }

    private void Awake()
    {
        originalColor = renderer.material.color;
    }

    public void Cook(CookingType type, float deltaTime)
    {
        if (percentCooked < 1f && type == cookingMethod)
            percentCooked += deltaTime * cookingRate;
        else
            percentMiscooked += deltaTime * cookingRate;

        UpdateVisuals();
    }

    private void UpdateVisuals()
    {
        Color newColor = Color.Lerp(originalColor, cookedColor, percentCooked);
        renderer.material.color = Color.Lerp(newColor, burntColor, percentMiscooked);
    }
}
