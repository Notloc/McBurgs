using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerToolTip : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] InteractionManager interactionManager;

    private void LateUpdate()
    {
        IInteractable target = interactionManager.Target;

        if (target != null)
            this.transform.position = target.transform.position;
        else
            this.transform.position = new Vector3(0f, -9999f, 0f);
        
    }
}
