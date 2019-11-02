using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] string gameSceneName;

    public void StartGame(ushort port = 7777)
    {
        SceneManager.LoadScene(gameSceneName);
    }
}
