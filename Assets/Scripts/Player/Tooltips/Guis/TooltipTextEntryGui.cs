using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TooltipTextEntryGui : BaseTooltipEntryGui
{
    [Header("Reuqired References")]
    [SerializeField] Text infoText;

    public override void AssignEntry(TooltipEntry entry)
    {
        this.entry = entry;

        UpdateDisplay();
    }

    public override void UpdateDisplay()
    {
        infoText.text = entry.text;
    }
}
