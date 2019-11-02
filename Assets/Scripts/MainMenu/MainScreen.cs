using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainScreen : MenuScreen
{
    [Header("Buttons")]
    [SerializeField] Button playButton;
    [SerializeField] Button mpButton;
    [SerializeField] Button exitButton;
    [Space]
    [SerializeField] MenuScreen mpScreen;

    protected override void Awake()
    {
        base.Awake();

        playButton.onClick.AddListener(OnPlay);
        mpButton.onClick.AddListener(OnMp);
        exitButton.onClick.AddListener(OnExit);
    }


    private void OnPlay()
    {
        menuController.StartGame();
    }

    private void OnMp()
    {
        ChangeScreen(mpScreen);
    }

    private void OnExit()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
