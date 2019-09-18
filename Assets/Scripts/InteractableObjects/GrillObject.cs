using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrillObject : MonoBehaviour, IInteractable, IHaveTooltip
{
    [SerializeField] TooltipData tooltipData;
    [SerializeField] Vector3 toolTipDisplayOffset;

    public Vector3 DisplayOffset { get { return this.transform.rotation * toolTipDisplayOffset; } }
    public TooltipData TooltipData { get { return tooltipData; } }

    public bool IsOn { get { return isOn; } }
    bool isOn = false;
    HashSet<ICookable> grillingItems = new HashSet<ICookable>();

    private void FixedUpdate()
    {
        GrillItems();
    }

    private void GrillItems()
    {
        if (isOn == false)
            return;

        foreach(ICookable item in grillingItems)
        {
            item.Cook(CookingType.Grill, Time.fixedDeltaTime);
        }
    }

    // ON/OFF CODE
    public void Interact()
    {
        ToggleOn();
    }

    private void ToggleOn()
    {
        if (isOn)
            TurnOff();
        else
            TurnOn();
    }
    private void TurnOn()
    {
        if (isOn)
            return;

        isOn = true;
    }
    private void TurnOff()
    {
        if (!isOn)
            return;

        isOn = false;
    }
    //

    // COLLISION CODE
    private void OnTriggerEnter(Collider other)
    {
        ICookable cookable = other.GetComponentInParent<ICookable>();
        if (cookable == null)
            return;

        if (grillingItems.Contains(cookable) == false)
            grillingItems.Add(cookable);
    }
    private void OnTriggerExit(Collider other)
    {
        ICookable cookable = other.GetComponentInParent<ICookable>();
        if (cookable == null)
            return;

        if (grillingItems.Contains(cookable))
            grillingItems.Remove(cookable);
    }
    //
}
