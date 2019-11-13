using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Button resumeButton;

    public bool IsPaused { get; private set; }

    private void Awake()
    {
        resumeButton.onClick.AddListener(ResumeGame);
    }

    public void ResumeGame()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        ClientController.Instance.Player.Unpause();
        IsPaused = false;
    }

    public void PauseGame()
    {
        this.gameObject.SetActive(true);
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        ClientController.Instance.Player.Pause();
        IsPaused = true;
    }
}
