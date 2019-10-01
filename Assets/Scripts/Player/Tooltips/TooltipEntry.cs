using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct TooltipEntry
{
    public enum TooltipEntryType
    {
        Text,
        Value,
        ProgressBar,
        BurgerScore
    }

    public TooltipEntryType entryType;
    public string text;
    public string valueName;
    public Component scriptReference;
}
