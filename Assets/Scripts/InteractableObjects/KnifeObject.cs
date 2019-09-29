using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnifeObject : ItemObject, IUsable
{
    [Header("Options")]
    [SerializeField] float bladeActiveTime;
    [Space]
    [SerializeField] Vector3 useOffset = Vector3.zero;
    [SerializeField] Quaternion useRotation = Quaternion.identity;
    [SerializeField] bool resetRotationAfterUse = true;

    public Vector3 UseOffset { get { return useOffset; } }
    public Quaternion UseRotation { get { return useRotation; } }
    public bool ResetRotationAfterUse { get { return resetRotationAfterUse; } }

    bool knifeEnabled = false;

    public UnityAction OnEnableEvent { get; set; }
    public UnityAction OnDisableEvent { get; set; }

    public void EnableUse()
    {
        knifeEnabled = true;
        OnEnableEvent?.Invoke();

        StartCoroutine(UseTimer(bladeActiveTime));
    }

    IEnumerator UseTimer(float timer)
    {
        float start = Time.time;
        yield return null;

        while (start + timer < Time.time)
        {
            yield return null;
        }
        DisableUse();
    }

    public void DisableUse()
    {
        knifeEnabled = false;
        OnDisableEvent?.Invoke();
        StopAllCoroutines();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!knifeEnabled)
            return;

        if (LayerManager.InteractionLayer != collision.collider.gameObject.layer)
            return;

        ICuttable cuttable = collision.collider.GetComponentInParent<ICuttable>();
        if (InterfaceUtil.IsNull(cuttable) == false)
        {
            cuttable.Cut(collision);
        }
    }
}
