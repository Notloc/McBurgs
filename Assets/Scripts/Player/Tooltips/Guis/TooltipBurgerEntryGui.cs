using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
 class TooltipBurgerEntryGui : BaseTooltipEntryGui
{
    [Header("Required References")]
    [SerializeField] ProgressBar vegetarianBar;
    [SerializeField] ProgressBar cheesinessBar;

    public override void AssignEntry(TooltipEntry entry)
    {
        this.entry = entry;
        UpdateDisplay();
    }

    public override void UpdateDisplay()
    {
        // Try to get the value via reflection
        try
        {
            Type reflectionType = entry.scriptReference.GetType();
            var reflectedProperty = reflectionType.GetProperty(entry.valueName);

            var value = reflectedProperty.GetValue(entry.scriptReference);
            UpdateScores((BurgerScore)value);
        }
        catch (System.Exception e)
        {
            if (!entry.scriptReference)
                Debug.LogError("No script reference provided for the entry.");
            else
                Debug.LogError(entry.valueName + " was not found in " + entry.scriptReference.name);
        }
    }

    private void UpdateScores(BurgerScore score)
    {
        vegetarianBar.UpdateProgress(score.vegetarianScore);
        cheesinessBar.UpdateProgress(score.chessiness);
    }
}
