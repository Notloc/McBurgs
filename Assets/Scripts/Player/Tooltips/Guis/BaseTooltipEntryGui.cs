using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseTooltipEntryGui : MonoBehaviour
{
    protected TooltipEntry entry;

    public abstract void AssignEntry(TooltipEntry entry);
    public abstract void UpdateDisplay();
}
