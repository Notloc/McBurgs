using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PauseView : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] Button resumeButton;

    public bool IsPaused { get; private set; }

    private Player player;

    private void Awake()
    {
        player = GameObject.FindGameObjectWithTag(TagManager.Player).GetComponent<Player>();
        resumeButton.onClick.AddListener(ResumeGame);
    }

    public void ResumeGame()
    {
        this.gameObject.SetActive(false);
        Time.timeScale = 1f;

        player.Unpause();
        IsPaused = false;
    }

    public void PauseGame()
    {
        this.gameObject.SetActive(true);
        Time.timeScale = 0f;

        player.Pause();
        IsPaused = true;
    }
}
