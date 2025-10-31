using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        SceneManager.LoadScene("Lv1");
    }
    public void PauseGame()
    {
        Time.timeScale = 0f;
        Debug.Log("Game Paused!");
    }

    public void ContinueGame()
    {
        Time.timeScale = 1f;
        Debug.Log("Game Continued!");
    }

    public void ExitGame()
    {
        Debug.Log("Thoát game!");
        Application.Quit();
    }
}
