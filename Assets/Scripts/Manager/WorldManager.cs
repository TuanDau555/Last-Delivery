using System;
using TMPro;
using UnityEngine;

public class WorldManager : SingletonPersistent<WorldManager>, ISaveable
{
    #region KEYS & NAME
    private const string CURRENT_MONEY = "CurrentMoney";
    private const string CURRENT_DAY = "CurrentDay";
    private const string CURRENT_COLLECTED = "CurrentCollected";
    private const string MONEY = "Money Count";
    private const string DAYS = "Day Count";
    private const string COLLECTED = "Collected Count";
    #endregion
    
    #region Parameters
    [SerializeField] public int _money;
    [SerializeField] private int _currentDay;
    [SerializeField] private int _itemLostCount;
    private int currentLevel;

    [Space(10)]
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI currentDayText;
    [SerializeField] private TextMeshProUGUI currentMoneyText;
    [SerializeField] private GameObject lostObjectUI;
    [SerializeField] private TextMeshProUGUI itemLostCountText;
    [SerializeField] private TextMeshProUGUI totalItemCountText;
    #endregion

    #region Execute
    public override void Awake()
    {
        base.Awake();
        RefreshUI();
    }
    
    void Start()
    {
        DeliveryManager.Instance.OnDeliverySuccess += AddMoney;
        DeliveryManager.Instance.OnDeliverySuccess += NextDay;
        DeliveryManager.Instance.OnDeliverySuccess += IncreaseLostItemFound;
    }

    void Update()
    {

    }

    void OnDisable()
    {
        DeliveryManager.Instance.OnDeliverySuccess -= AddMoney;
        DeliveryManager.Instance.OnDeliverySuccess -= NextDay;      
        DeliveryManager.Instance.OnDeliverySuccess -= IncreaseLostItemFound;
    }
    #endregion

    #region Update World
    /// <summary>
    /// Event Add money
    /// </summary>
    private void AddMoney(object sender, EventArgs e)
    {
        _money += 10;
        UpdateDayUI();
    }

    /// <summary>
    /// Collect enough amount off money
    /// </summary>
    private void NextDay(object sender, EventArgs e)
    {
        _currentDay++;
        UpdateDayUI();
    }

    /// <summary>
    /// Call func this when the player buys an object
    /// </summary>
    /// <param name="amount">Price of the object</param>
    public bool SpendMoney(int amount)
    {
        if (_money < amount) return false;
        _money = Mathf.Max(0, _money - amount); // prevent negative value
        UpdateMoneyUI();

        return true;
    }
    
    public void IncreaseLostItemFound(object sender, EventArgs e)
    {
        _itemLostCount++;
        UpdateLostUI();
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
        if (_currentDay < 7)
        {
            Hide();
        }
        else
        {
            Show();
            DeliveryManager.Instance.isOpenLv2 = true;
        }

        if (currentDayText == null || currentMoneyText == null)
        {
            currentDayText = GameObject.Find(DAYS)?.GetComponent<TextMeshProUGUI>();
            currentMoneyText = GameObject.Find(MONEY)?.GetComponent<TextMeshProUGUI>();
        }

        UpdateDayUI();
        UpdateMoneyUI();
        UpdateLostUI();
    }
    
    private void UpdateMoneyUI()
    {
        if (currentMoneyText != null)
            currentMoneyText.text = _money.ToString();
    }

    private void UpdateDayUI()
    {
        if (currentDayText != null)
            currentDayText.text = _currentDay.ToString();
    }

    /// <summary>
    /// Lost Object Collected UI
    /// </summary>
    private void UpdateLostUI()
    {
        if (lostObjectUI == null) return;

        if (itemLostCountText != null)
            itemLostCountText.text = _itemLostCount.ToString();

        if (totalItemCountText != null)
            totalItemCountText.text = DeliveryManager.Instance.totalLostItemCount.ToString();
    }

    private void Show() => lostObjectUI?.SetActive(true);
    private void Hide() => lostObjectUI?.SetActive(false);
    #endregion
}