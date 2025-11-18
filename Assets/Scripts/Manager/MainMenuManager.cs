using UnityEngine;
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
    }

    public void ContinueGame()
    {
        SaveManager.Instance.LoadGame();
    }

    public void QuitGame()
    {
        Application.Quit();
    }
    #endregion

    #region Main Menu UI
    private void HideContinueBtn()
    {
        continueBtn.interactable = SaveManager.Instance.HasSaveFile();
    }
    #endregion
}