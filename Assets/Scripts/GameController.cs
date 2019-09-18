using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Prefabs")]
    [SerializeField] Player playerPrefab;
    [SerializeField] GuiController guiPrefab;

    [Header("Options")]
    [SerializeField] Vector3 spawnPosition;

    public Player Player { get; private set; }
    public GuiController Gui { get; private set; }

    private void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;

        Player = Instantiate(playerPrefab, spawnPosition, Quaternion.identity);
        Gui = Instantiate(guiPrefab);
    }
}
