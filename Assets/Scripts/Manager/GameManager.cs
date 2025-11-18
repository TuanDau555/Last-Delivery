using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameObject gamePausePanel;
    
    private InputManager _inputManager;

    #region Execute
    void Start()
    {
        _inputManager = InputManager.Instance;
    }

    void Update()
    {
        PauseGame();
    }
    #endregion
    
    #region Pause menu
    public void RestartDay()
    {
        Time.timeScale = 1f;
        SceneManager.LoadSceneAsync("Lv1");
        SaveManager.Instance.LoadCheckpoint();
    }

    public void ResumeGame()
    {
        Time.timeScale = 1f;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.None;
        gamePausePanel.SetActive(false);
    }
    
    public void PauseGame()
    {
        if (_inputManager.IsPause())
        {
            Time.timeScale = 0f;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
            gamePausePanel.SetActive(true);    
        }
    }

    public void BackToMenu()
    {
        SceneManager.LoadScene("Menu");
    }    
    #endregion
}
