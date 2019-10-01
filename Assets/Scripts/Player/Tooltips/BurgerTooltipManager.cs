using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurgerTooltipManager : TooltipManager
{
    [SerializeField] FoodObject bun;
    [SerializeField] BurgerScorer scorer;

    public override IHaveTooltip ActiveTooltip { get; protected set; }

    private void Awake()
    {
        ActiveTooltip = bun;
    }

    public void OnBurgerFinish()
    {
        ActiveTooltip = scorer;
    }
}
