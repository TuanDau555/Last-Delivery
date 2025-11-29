using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{

    [SerializeField] private GameObject gamePausePanel;
    [SerializeField] private GameObject gameOverPanel;
    
    private InputManager _inputManager;

    #region Execute
    private void Start()
    {
        _inputManager = InputManager.Instance;
        gameOverPanel.SetActive(false);
        gamePausePanel.SetActive(false);
    }

    private void Update()
    {
        PauseGame();
    }
    #endregion
    
    #region End Game
    public void GameOver()
    {
        Time.timeScale = 0f;
        gameOverPanel.SetActive(true);
        
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
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
        Cursor.lockState = CursorLockMode.Locked;
        gamePausePanel.SetActive(false);
    }
    
    public void PauseGame()
    {
        if(SceneManager.GetActiveScene().name == "Menu") return;
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
        // Refresh object's list to save correct object in current Scene
        SaveManager.Instance.RefreshSaveables();
        SaveManager.Instance.SaveGame();

        ResetCoreSingletons();
        SceneManager.LoadScene("Menu");
    }

    private void ResetCoreSingletons()
    {
        Destroy(WorldManager.Instance?.gameObject);
        Destroy(InputManager.Instance?.gameObject);
    }    
    #endregion
}
