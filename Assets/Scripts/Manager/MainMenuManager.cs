using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    #region KEYS
    private const string VSYNC_KEY = "VSync";
    #endregion
    
    [SerializeField] private Button continueBtn;
    [SerializeField] private Toggle vSynceToggle;

    #region Excute
    private void Start()
    {
        HideContinueBtn();
        LoadSetting();
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

    #region Game Setting
    public void OnVSyncToggleChanged(bool enabled)
    {
        QualitySettings.vSyncCount = enabled ? 1 : 0;

        Application.targetFrameRate = enabled ? 60 : -1;

        Debug.Log("VSync: " + QualitySettings.vSyncCount);
        Debug.Log("Target FPS: " + Application.targetFrameRate);
    }
    #endregion

    #region Save/Load Setting
    public void SaveSetting()
    {
        PlayerPrefs.SetInt(VSYNC_KEY, QualitySettings.vSyncCount);
        PlayerPrefs.Save();
    }

    public void LoadSetting()
    {
        int v = PlayerPrefs.GetInt(VSYNC_KEY, 1); // default = ON

        QualitySettings.vSyncCount = v;
        Application.targetFrameRate = (v > 0) ? 60 : -1;

        vSynceToggle.SetIsOnWithoutNotify(v > 0);
    }
    #endregion
}