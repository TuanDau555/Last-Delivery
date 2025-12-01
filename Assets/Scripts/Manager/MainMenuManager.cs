using System.Collections;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuManager : MonoBehaviour
{
    #region KEYS
    private const string VSYNC_KEY = "VSync";
    private const string MOUSENS_KEY = "MouseSensitivity";
    private const string MUSIC_KEY = "music";
    #endregion
    
    #region Parameter 
    [Header("Player Stats")]
    [SerializeField] private CharacterStatsSO playerStatsSO;

    [Space(10)]
    [Header("UI")]
    [SerializeField] private Button continueBtn;
    [SerializeField] private Toggle vSynceToggle;
    [SerializeField] private Slider mouseSensitiveSlider;
    [SerializeField] private Animator scenceTransition;

    [Space(10)]
    [Header("Music")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    #endregion

    #region Excute
    private void Awake()
    {
    }
    private void Start()
    {
        HideContinueBtn();
        LoadSetting();
        mouseSensitiveSlider.maxValue = 100f;
    }
    #endregion

    #region Main Menu Button
    public void CreateNewGame()
    {
        SaveManager.Instance.NewGame();
        scenceTransition.SetTrigger("Start Crossfade");
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
        scenceTransition.SetTrigger("Start Crossfade");
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
    }

    public void OnMouseSensitivityChanged(float value)
    {
        PlayerPrefs.SetFloat(MOUSENS_KEY, value);
        PlayerPrefs.Save();

        playerStatsSO.stats.lookSensitive = value;
    }
    
    public void OnMusicChanged()
    {
        float volume = musicSlider.value;
        PlayerPrefs.SetFloat(MUSIC_KEY, volume);
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
        // Vsync Setting 
        int vsync = PlayerPrefs.GetInt(VSYNC_KEY, 1); // default = ON
        QualitySettings.vSyncCount = vsync;
        Application.targetFrameRate = (vsync > 0) ? 60 : -1;
        vSynceToggle.SetIsOnWithoutNotify(vsync > 0);

        // Mouse Setting
        float mouseSen = PlayerPrefs.GetFloat(MOUSENS_KEY, 1.0f); // default = 1
        playerStatsSO.stats.lookSensitive = mouseSen;
        if (mouseSensitiveSlider != null)
            mouseSensitiveSlider.SetValueWithoutNotify(mouseSen);
    }
    #endregion
}