using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingSystem : MonoBehaviour
{
    [SerializeField] float targetingDistance = 2f;
    [SerializeField] LayerMask targetingMask = 0;

    private Transform cameraT;
    private GameObject target;

    private void Start()
    {
        cameraT = Camera.main.transform.transform.transform.transform.transform.transform;
    }

    public T GetTargetComponent<T>() where T : MonoBehaviour
    {
        RaycastForTarget();
        if (!target)
            return null;
        return target.GetComponent<T>();
    }

    private void RaycastForTarget()
    {
        RaycastHit hitInfo;
        Ray ray = new Ray(cameraT.position, cameraT.forward);

        bool hitSomething = Physics.Raycast(ray, out hitInfo, targetingDistance, targetingMask);
        if (hitSomething)
        {
            Rigidbody rigidbody = hitInfo.rigidbody;
            target = rigidbody != null ? rigidbody.gameObject : null;
        }
        else
        {
            target = null;
        }
    }
}
