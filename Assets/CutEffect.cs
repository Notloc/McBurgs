﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CutEffect : MonoBehaviour, ICuttable
{
    [SerializeField] GameObject effectPrefab;
    [SerializeField] float effectCooldown = 0.5f;

    private float lastEffectTime = 0f;

    public void Cut(Collision collision)
    {
        ContactPoint contact = collision.GetContact(0);
        TryEffect(contact);
    }

    private void TryEffect(ContactPoint contact)
    {
        // No effect if its too soon since the previous
        if (Time.time < lastEffectTime + effectCooldown)
            return;

        lastEffectTime = Time.time;
        Instantiate(effectPrefab, contact.point, Quaternion.LookRotation(contact.normal));
    }
}
