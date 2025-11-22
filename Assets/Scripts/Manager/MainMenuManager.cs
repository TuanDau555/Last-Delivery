using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    [SerializeField] private Button continueBtn;

    #region Excute
    private void Start()
    {
        HideContinueBtn();
    }
    #endregion

    #region Main Menu Button
    public void CreateNewGame()
    {
        SaveManager.Instance.NewGame();
        SceneManager.LoadScene("LV1");
    }

    public void ContinueGameBtn()
    {
        if(!SaveManager.Instance.HasSaveFile()) return;
        StartCoroutine(LoadAndContinue());
        Time.timeScale = 1f;
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    private IEnumerator LoadAndContinue()
    {
        SceneManager.sceneLoaded += OnSceneLoad;

        yield return SceneManager.LoadSceneAsync("LV1");
        
    }
    #endregion

    #region Main Menu UI
    private void HideContinueBtn()
    {
        continueBtn.interactable = SaveManager.Instance.HasSaveFile();
    }
    #endregion

    #region LoadScene
    private void OnSceneLoad(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoad;

        // Refresh to find object in when load scene
        SaveManager.Instance.RefreshSaveables();

        SaveManager.Instance.LoadGame();
    }
    #endregion
}