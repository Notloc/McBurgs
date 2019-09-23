using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnifeObject : ItemObject, IUsable
{
    [Header("Options")]
    [SerializeField] Vector3 useOffset = Vector3.zero;
    [SerializeField] Quaternion useRotation = Quaternion.identity;

    public Vector3 UseOffset { get { return useOffset; } }
    public Quaternion UseRotation { get { return useRotation; } }
    public bool CutEnabled { get; private set; }

    public UnityAction OnEnableEvent { get; set; }
    public UnityAction OnDisableEvent { get; set; }

    bool enableBlade = false;

    public void EnableUse()
    {
        CutEnabled = true;
        OnEnableEvent?.Invoke();
    }

    public void DisableUse()
    {
        CutEnabled = false;
        OnDisableEvent?.Invoke();
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
