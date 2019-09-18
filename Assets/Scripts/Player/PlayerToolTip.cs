using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerTooltip : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] InteractionManager interactionManager;
    [SerializeField] Canvas tooltipCanvas;

    IHaveTooltip currentTarget;

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
        if (currentTarget == null)
        {
            tooltipCanvas.gameObject.SetActive(false);
            return;
        }
        else
            tooltipCanvas.gameObject.SetActive(true);

        // Position the tooltip
        tooltipCanvas.transform.position = currentTarget.transform.position + currentTarget.DisplayOffset;
    }

    private void PopulateTooltip(IHaveTooltip target)
    {

    }
}
