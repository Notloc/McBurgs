using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TooltipManager : MonoBehaviour
{
    public abstract IHaveTooltip ActiveTooltip { get; protected set; }
}
