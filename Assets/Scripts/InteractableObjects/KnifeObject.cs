using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnifeObject : ItemObject, IUsable
{
    [Header("Options")]
    [SerializeField] Vector3 useOffset = Vector3.zero;

    public Vector3 UseOffset { get { return useOffset; } }
    public bool CutEnabled { get; private set; }

    bool enableBlade = false;

    public void EnableUse()
    {
        CutEnabled = true;
    }

    public void DisableUse()
    {
        CutEnabled = false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        ICuttable cuttable = collision.collider.GetComponentInParent<ICuttable>();
        if (InterfaceUtil.IsNull(cuttable) == false)
        {
            cuttable.Cut(collision);
        }
    }
}
