using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : Singleton<UIManager>
{
    #region Player/Cat Stats
    [SerializeField] private TextMeshProUGUI moneyCount;
    [SerializeField] private Slider catMoodBar;
    [SerializeField] private Slider playerHealth;
    #endregion

    [ContextMenu("Reduce cat mood bar")]
    public void CatMoodBarHandle()
    {
        if (catMoodBar.value >= 50)
        {
            Debug.Log("Reduce cat health");
            catMoodBar.value -= 10;
        }
        else if (catMoodBar.value <= 20)
        {
            Debug.Log("Increase cat health");
            catMoodBar.value += 10;
        }
    }
}
