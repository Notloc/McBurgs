using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

[RequireComponent(typeof(PhysicsComponent))]
public class JointConnectionComponent : MonoBehaviour
{
    [SerializeField] List<Transform> jointConnectionPoints = new List<Transform>();

    public PhysicsComponent Physics { get; private set; }

    private void Awake()
    {
        Physics = GetComponent<PhysicsComponent>();
    }

    public Vector3 GetClosestConnectionPoint(Vector3 position)
    {
        if (jointConnectionPoints.Count == 0)
        {
            return transform.position;
        }
        return GetClosestTransform(jointConnectionPoints, position).position;
    }

    // TODO: Move to a util class
    private static Transform GetClosestTransform(IList<Transform> transforms, Vector3 position)
    {
        if (transforms == null || transforms.Count == 0)
        {
            return null;
        }

        Transform closest = transforms[0];
        float closestDistance = Vector3.SqrMagnitude(position - closest.position);
        for (int i = 1; i < transforms.Count; i++)
        {
            float distance = Vector3.SqrMagnitude(position - transforms[i].position);
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closest = transforms[i];
            }
        }
        return closest;
    }
}
