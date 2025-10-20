using System;
using TMPro;
using UnityEngine;

public class WorldManager : Singleton<WorldManager>, ISaveable
{
    #region KEYS
    private const string CURRENT_MONEY = "CurrentMoney";
    private const string CURRENT_DAY = "CurrentDay";
    #endregion
    
    #region Parameters
    [SerializeField] public int _money;
    [SerializeField] private int _currentDay;
    private int currentLevel;

    [Space(10)]
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI currentDayText;
    [SerializeField] private TextMeshProUGUI currentMoneyText;
    #endregion

    #region Execute
    void Start()
    {
        DeliveryManager.Instance.OnDeliverySuccess += AddMoney;
        DeliveryManager.Instance.OnDeliverySuccess += NextDay;
        RefreshUI();
    }

    void Update()
    {

    }

    void OnDisable()
    {
        DeliveryManager.Instance.OnDeliverySuccess -= AddMoney;
        DeliveryManager.Instance.OnDeliverySuccess -= NextDay;      
    }
    #endregion

    #region Update World
    [ContextMenu("Add Money")]
    private void AddMoney(object sender, EventArgs e)
    {
        _money += 10;
        currentMoneyText.text = _money.ToString();
    }

    [ContextMenu("Next Day")]
    private void NextDay()
    {
        _currentDay++;
        currentDayText.text = _currentDay.ToString();
    }
    #endregion

    #region Save and Load
    public void Save(SaveData data)
    {
        data.Set(CURRENT_DAY, _currentDay);
        data.Set(CURRENT_MONEY, _money);
    }

    public void Load(SaveData data)
    {
        _currentDay = data.Get<int>(CURRENT_DAY, this._currentDay);
        _money = data.Get<int>(CURRENT_MONEY, this._money);

        RefreshUI();
    }
    #endregion

    #region UI
    private void RefreshUI()
    {
        if (currentDayText != null)
            currentDayText.text = _currentDay.ToString();
        if (currentMoneyText != null)
            currentMoneyText.text = _money.ToString();
    }
    #endregion
}