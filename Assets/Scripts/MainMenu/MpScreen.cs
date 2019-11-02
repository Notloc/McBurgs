using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MpScreen : MenuScreen
{
    [Header("Buttons")]
    [SerializeField] Button backButton;

    protected override void Awake()
    {
        base.Awake();
        backButton.onClick.AddListener(OnBack);
    }

    private void OnBack()
    {
        PreviousScreen();
    }
}
