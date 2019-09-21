using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttableFood : MonoBehaviour, ICuttable
{
    [Header("Required References")]
    [SerializeField] FoodObject cutVersionOfFoodPrefab;

    [Header("Options")]
    [SerializeField] int amountProduced;
    [SerializeField] Vector3 spreadStart;
    [SerializeField] Vector3 spreadEnd;

    public void Cut(Collision collision)
    {
        for (int i=0; i < amountProduced; i++)
        {
            Vector3 offset = Vector3.Lerp(spreadStart, spreadEnd, i / (float)amountProduced);
            Instantiate(cutVersionOfFoodPrefab, this.transform.position + (this.transform.rotation * offset), this.transform.rotation);
        }

        Destroy(this.gameObject);
    }
}
