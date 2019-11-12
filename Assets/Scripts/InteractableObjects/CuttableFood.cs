using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class CuttableFood : NetworkBehaviour, ICuttable
{
    [Header("Required References")]
    [SerializeField] FoodObject cutVersionOfFoodPrefab;

    [Header("Options")]
    [SerializeField] int amountProduced;
    [SerializeField] Vector3 spreadStart;
    [SerializeField] Vector3 spreadEnd;

    public override void OnStartClient()
    {
        base.OnStartClient();
        ClientScene.RegisterPrefab(cutVersionOfFoodPrefab.gameObject);
    }

    [Server]
    public void Cut(Collision collision)
    {
        for (int i=0; i < amountProduced; i++)
        {
            Vector3 offset = Vector3.Lerp(spreadStart, spreadEnd, i / (float)amountProduced);
            var obj = Instantiate(cutVersionOfFoodPrefab, this.transform.position + (this.transform.rotation * offset), this.transform.rotation);
            NetworkServer.Spawn(obj.gameObject);
        }

        NetworkServer.Destroy(this.gameObject);
        Destroy(this.gameObject);
    }
}
