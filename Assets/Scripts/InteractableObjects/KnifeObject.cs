using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeObject : ItemObject, IUsable
{
    [Header("Knife")]
    [Header("Require References")]
    [SerializeField] Collider cuttingTrigger;

    [Header("Options")]
    [SerializeField] Vector3 useOffset = Vector3.zero;

    public Vector3 UseOffset { get { return useOffset; } }

    public void EnableUse()
    {
        cuttingTrigger.enabled = true;
    }

    public void DisableUse()
    {
        cuttingTrigger.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        ICuttable cuttable = other.GetComponentInParent<ICuttable>();
        if (cuttable != null)
            cuttable.Cut();
    }
}
