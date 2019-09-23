using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
 class TooltipProgressEntryGui : BaseTooltipEntryGui
{
    [Header("Required References")]
    [SerializeField] Text infoText;
    [SerializeField] ProgressBar progressBar;

    public override void AssignEntry(TooltipEntry entry)
    {
        this.entry = entry;

        UpdateDisplay();
    }

    public override void UpdateDisplay()
    {
        infoText.text = entry.text;

        // Try to get the value via reflect and display it as text
        try
        {
            Type reflectionType = entry.scriptReference.GetType();
            var reflectedProperty = reflectionType.GetProperty(entry.valueName);

            var value = reflectedProperty.GetValue(entry.scriptReference);
            progressBar.UpdateProgress((float)value);
        }
        catch (System.Exception e)
        {
            if (!entry.scriptReference)
                Debug.LogError("No script reference provided for the entry.");
            else
                Debug.LogError(entry.valueName + " was not found in " + entry.scriptReference.name);
        }
    }
}
