using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class CookingSystem : MonoBehaviour
{
    [SerializeField] float maxCookingOutput = 5f;
    [SerializeField] float singleCookingOutput = 1f;

    [SerializeField] List<CookableObject> cookableObjects = new List<CookableObject>();

    // TODO: Replace with a coroutine that is started when the cookable list is not empty
    private void Update()
    {
        Cook();
    }

    private void Cook()
    {
        float cookingAmount = CalculateCookingOutput();
        foreach (CookableObject cookable in cookableObjects)
        {
            cookable.Cook(cookingAmount);
        }
    }

    private float CalculateCookingOutput()
    {
        return Mathf.Clamp(maxCookingOutput / cookableObjects.Count, 0f, singleCookingOutput) * Time.deltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        CookableObject cookable = GetCookableObject(other);
        if (!cookable)
            return;

        AddCookable(cookable);
    }

    private void OnTriggerExit(Collider other)
    {
        CookableObject cookable = GetCookableObject(other);
        if (!cookable)
            return;

        RemoveCookable(cookable);
    }

    private CookableObject GetCookableObject(Collider collider)
    {
        return collider.GetComponentInParent<CookableObject>();
    }

    private void AddCookable(CookableObject cookable)
    {
        if (!cookableObjects.Contains(cookable))
        {
            cookableObjects.Add(cookable);
        }
    }

    private void RemoveCookable(CookableObject cookable)
    {
        cookableObjects.Remove(cookable);
    }
}
