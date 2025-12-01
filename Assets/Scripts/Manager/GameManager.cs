using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : Singleton<GameManager>
{
    private const float fadeDuration = 0.3f;

    [SerializeField] private GameObject gamePausePanel;
    [SerializeField] private GameObject gameOverPanel;
    [SerializeField] private Animator sceneTransition;
    
    private InputManager _inputManager;

    #region Execute
    private void Start()
    {
        if(SceneManager.GetActiveScene().name == "Menu") return;

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
        StartCoroutine(FadeBackToMenu());
        Time.timeScale = 1f;
    }

    private IEnumerator FadeBackToMenu()
    {
        sceneTransition.SetTrigger("Start Crossfade");

        yield return new WaitForSeconds(fadeDuration);

        AsyncOperation async = SceneManager.LoadSceneAsync("Menu");

        while(!async.isDone) yield return null;

        sceneTransition = GameObject.Find("Scene Loader").GetComponentInChildren<Animator>();
        sceneTransition.Play("Crossfade_End");
    }
    
    private void ResetCoreSingletons()
    {
        Destroy(WorldManager.Instance?.gameObject);
        Destroy(InputManager.Instance?.gameObject);
    }    
    #endregion
}
