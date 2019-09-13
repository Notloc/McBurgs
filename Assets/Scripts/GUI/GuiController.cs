using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuiController : MonoBehaviour
{
    [Header("Required References")]
    [SerializeField] PauseView pauseView;


    void Update()
    {
        bool pressedPause = Input.GetButtonDown(ControlBindings.PAUSE);

        if (pressedPause)
        {
            if (pauseView.IsPaused)
                pauseView.ResumeGame();
            else
                pauseView.PauseGame();
        }

    }
}
