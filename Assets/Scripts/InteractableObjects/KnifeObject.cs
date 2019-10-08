using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KnifeObject : ItemObject, IUsable
{
    [Header("Required References")]
    [SerializeField] Collider regularCollisionBox;
    [SerializeField] Collider bladeCollisionBox;

    [Header("Options")]
    [SerializeField] float bladeActiveTime;
    [Space]
    [SerializeField] Vector3 useOffset = Vector3.zero;
    [SerializeField] Quaternion useRotation = Quaternion.identity;
    [SerializeField] bool ignorePositionSmoothing = true;
    [SerializeField] bool resetRotationAfterUse = true;

    public Vector3 UseOffset { get { return useOffset; } }
    public Quaternion UseRotation { get { return useRotation; } }
    public bool IgnorePositionSmoothing { get { return ignorePositionSmoothing; } }
    public bool ResetRotationAfterUse { get { return resetRotationAfterUse; } }

    bool knifeEnabled = false;

    public UnityAction OnEnableEvent { get; set; }
    public UnityAction OnDisableEvent { get; set; }

    private void Awake()
    {
        bladeCollisionBox.enabled = false;
        regularCollisionBox.enabled = true;
    }

    public void EnableUse()
    {
        knifeEnabled = true;
        OnEnableEvent?.Invoke();

        bladeCollisionBox.enabled = true;
        regularCollisionBox.enabled = false;

        StartCoroutine(UseTimer(bladeActiveTime));
    }

    IEnumerator UseTimer(float timer)
    {
        float start = Time.time;
        yield return null;

        while (start + timer > Time.time)
        {
            yield return null;
        }
        DisableUse();
    }

    public void DisableUse()
    {
        knifeEnabled = false;
        OnDisableEvent?.Invoke();

        bladeCollisionBox.enabled = false;
        regularCollisionBox.enabled = true;

        StopAllCoroutines();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!knifeEnabled)
            return;

        if (LayerManager.InteractionLayer != collision.collider.gameObject.layer)
            return;

        ICuttable cuttable = collision.collider.GetComponentInParent<ICuttable>();
        if (cuttable.IsNull() == false)
        {
            cuttable.Cut(collision);
        }
    }
}
