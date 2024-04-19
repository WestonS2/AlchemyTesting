using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public void LoadMenu()
    {
        //SceneManager.LoadScene(MenuScene);
    }

    public void QuitGame()
    {
        print("Pretend the game is quitting");
        Application.Quit();
    }
}
