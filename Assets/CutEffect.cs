using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutEffect : MonoBehaviour, ICuttable
{
    [SerializeField] GameObject effectPrefab;

    public void Cut(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        Instantiate(effectPrefab, contact.point, Quaternion.LookRotation(contact.normal));
    }
}
