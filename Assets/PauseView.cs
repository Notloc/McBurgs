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

    private void OnEnable()
    {
        _PauseGame();
    }
    private void OnDisable()
    {
        _ResumeGame();
    }

    private void _PauseGame()
    {
        Time.timeScale = 0f;
        IsPaused = true;
    }

    private void _ResumeGame()
    {
        Time.timeScale = 1f;
        IsPaused = false;
    }

    public void ResumeGame()
    {
        this.gameObject.SetActive(false);
    }

    public void PauseGame()
    {
        this.gameObject.SetActive(true);
    }
}
