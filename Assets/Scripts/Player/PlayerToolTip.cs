using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTooltip : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] TooltipTextEntryGui textEntryGuiPrefab;
    [SerializeField] TooltipValueEntryGui valueEntryGuiPrefab;
    [SerializeField] TooltipProgressEntryGui progressEntryGuiPrefab;
    [SerializeField] TooltipBurgerEntryGui burgerEntryGuiPrefab;

    [Header("Required References")]
    [SerializeField] Camera targetCamera;
    [SerializeField] InteractionManager interactionManager;
    [SerializeField] Canvas tooltipCanvas;
    [SerializeField] RectTransform entryParent;

    IHaveTooltip currentTarget;
    List<BaseTooltipEntryGui> entryGuis = new List<BaseTooltipEntryGui>();

    private void LateUpdate()
    {
        bool tooltipInput = Input.GetButtonDown(ControlBindings.TOOLTIP);

        // Try to get a new target
        IHaveTooltip newTarget = null;
        Collider targetCollider = interactionManager.TargetCollider;
        if (targetCollider)
        {
            var manager = targetCollider.GetComponentInParent<TooltipManager>();
            if (manager)
                newTarget = manager.ActiveTooltip;
            else
                newTarget = targetCollider.GetComponentInParent<IHaveTooltip>();
        }

        if (tooltipInput && InterfaceUtil.IsNull(newTarget) == false)
        {
            if (currentTarget != newTarget)
            {
                PopulateTooltip(newTarget);
                currentTarget = newTarget;
            }
            else
            {
                currentTarget = null;
            }
        }



        // Enable/Disable tooltip 
        if ( InterfaceUtil.IsNull(currentTarget) || currentTarget == interactionManager.HeldItem as IHaveTooltip)
        {
            currentTarget = null;
            tooltipCanvas.gameObject.SetActive(false);
            return;
        }
        else
            tooltipCanvas.gameObject.SetActive(true);

        // Position the tooltip
        tooltipCanvas.transform.position = currentTarget.transform.position + currentTarget.DisplayOffset;
        tooltipCanvas.transform.rotation = targetCamera.transform.rotation;
        UpdateEntries();
    }

    private void UpdateEntries()
    {
        foreach (var entry in entryGuis)
            entry.UpdateDisplay();
    }

    private void PopulateTooltip(IHaveTooltip target)
    {
        ClearTooltip();

        TooltipData data = target.TooltipData;

        entryGuis.Add(AddEntry(data.title));
        foreach(var entry in data.entries)
            entryGuis.Add(AddEntry(entry));
    }

    private BaseTooltipEntryGui AddEntry(TooltipEntry entry)
    {
        BaseTooltipEntryGui newEntry;
        if (entry.entryType == TooltipEntry.TooltipEntryType.Text)
            newEntry = Instantiate(textEntryGuiPrefab, entryParent);
        else if (entry.entryType == TooltipEntry.TooltipEntryType.Value)
            newEntry = Instantiate(valueEntryGuiPrefab, entryParent);
        else if (entry.entryType == TooltipEntry.TooltipEntryType.BurgerScore)
            newEntry = Instantiate(burgerEntryGuiPrefab, entryParent);
        else if (entry.entryType == TooltipEntry.TooltipEntryType.ProgressBar)
            newEntry = Instantiate(progressEntryGuiPrefab, entryParent);
        else
        {
            Debug.LogError("Error: There is no prefab for this type of Tooltip entry.");
            return null;
        }

        newEntry.AssignEntry(entry);
        return newEntry;
    }

    private void ClearTooltip()
    {
        foreach (Transform child in entryParent.transform)
            Destroy(child.gameObject);

        entryGuis.Clear();
    }
}
