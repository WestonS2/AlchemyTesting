using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuController : MonoBehaviour
{
    public static bool IsGamePaused = false;
    public KeyCode PauseKey = KeyCode.Escape;
    public GameObject PauseMenu;
    public GameObject Player;
    private string MenuScene;
    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(PauseKey))
        {
            if (IsGamePaused)
            {
                ResumeGame();
            }
            else { PauseGame(); }
        }
    }

    public void ResumeGame()
    {
        PauseMenu.SetActive(false);
        Time.timeScale = 1.0f;
        IsGamePaused = false;
        Player.SetActive(true);
    }

    public void PauseGame()
    {
        PauseMenu.SetActive(true);
        Time.timeScale = 0f;
        IsGamePaused = true;
        Player.SetActive(false); 
    }

    public void LoadMenu()
    {
        //Time.timeScale = 1.0f;
        ResumeGame();
        print("Pretend the menu is loading");
        //SceneManager.LoadScene(MenuScene);
    }

    public void QuitGame()
    {
        print("Pretend the game is quitting");
        Application.Quit();
    }
}
