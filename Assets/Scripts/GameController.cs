﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public static GameController Instance;

    [Header("Prefabs")]
    [SerializeField] Player playerPrefab;
    
    [Header("Options")]
    [SerializeField] Vector3 spawnPosition;
    [SerializeField] float startingCash = 100f;

    public Player Player { get; private set; }
   
    private void Awake()
    {
        Instance = this;
        Money = startingCash;
    }


    public float Money { get; private set; }
    public void IncreaseMoney(float amount)
    {
        if (amount < 0)
            return;

        Money += amount;
    }

    public bool DecreaseMoney(float amount)
    {
        if (amount > Money)
            return false;

        Money -= amount;
        return true;
    }
}
