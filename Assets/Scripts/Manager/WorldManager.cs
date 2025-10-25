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
    /// <summary>
    /// Event Add money
    /// </summary>
    private void AddMoney(object sender, EventArgs e)
    {
        _money += 10;
        currentMoneyText.text = _money.ToString();
    }

    /// <summary>
    /// Collect enough amount off money
    /// </summary>
    private void NextDay(object sender, EventArgs e)
    {
        _currentDay++;
        currentDayText.text = _currentDay.ToString();
    }

    /// <summary>
    /// Call func this when the player buys an object
    /// </summary>
    /// <param name="amount">Price of the object</param>
    public bool SpendMoney(int amount)
    {
        if (_money < amount) return false;
        if (_money < 0)
            _money = 0; // prevent negative number

        _money -= amount;
        currentMoneyText.text = _money.ToString();

        return true;
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
    /// <summary>
    /// Refresh money text and day text when player load save file
    /// </summary>
    private void RefreshUI()
    {
        if (currentDayText != null)
            currentDayText.text = _currentDay.ToString();
        if (currentMoneyText != null)
            currentMoneyText.text = _money.ToString();
    }
    #endregion
}