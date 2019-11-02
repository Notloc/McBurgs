using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuScreen : MonoBehaviour
{
    protected MainMenuController menuController;
    protected MenuScreen previous;

    protected virtual void Awake()
    {
        menuController = GetComponentInParent<MainMenuController>();
    }

    public void SetPrevious(MenuScreen screen)
    {
        previous = screen;
    }

    protected void ChangeScreen(MenuScreen newScreen)
    {
        newScreen.SetPrevious(this);

        newScreen.gameObject.SetActive(true);
        this.gameObject.SetActive(false);
    }

    protected void PreviousScreen()
    {
        previous.gameObject.SetActive(true);
        this.gameObject.SetActive(false);

        previous = null;
    }

}
