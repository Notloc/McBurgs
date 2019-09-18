using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTooltip : MonoBehaviour
{
    [Header("Prefab")]
    [SerializeField] TooltipTextEntryGui textEntryGuiPrefab;
    [SerializeField] TooltipValueEntryGui valueEntryGuiPrefab;
    [SerializeField] TooltipProgressEntryGui progressEntryGuiPrefab;

    [Header("Required References")]
    [SerializeField] InteractionManager interactionManager;
    [SerializeField] Canvas tooltipCanvas;
    [SerializeField] RectTransform entryParent;

    IHaveTooltip currentTarget;
    List<BaseTooltipEntryGui> entryGuis = new List<BaseTooltipEntryGui>();

    private void LateUpdate()
    {
        // Try to get a new target
        IHaveTooltip newTarget = null;
        Collider targetCollider = interactionManager.TargetCollider;
        if (targetCollider)
            newTarget = targetCollider.GetComponentInParent<IHaveTooltip>();

        if (newTarget != null && currentTarget != newTarget)
        {
            PopulateTooltip(newTarget);
            currentTarget = newTarget;
        }

        // Enable/Disable tooltip 
        if (currentTarget == null || currentTarget == (interactionManager.HeldItem as IHaveTooltip))
        {
            currentTarget = null;
            tooltipCanvas.gameObject.SetActive(false);
            return;
        }
        else
            tooltipCanvas.gameObject.SetActive(true);

        // Position the tooltip
        tooltipCanvas.transform.position = currentTarget.transform.position + currentTarget.DisplayOffset;
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
        else
            newEntry = Instantiate(progressEntryGuiPrefab, entryParent);

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
