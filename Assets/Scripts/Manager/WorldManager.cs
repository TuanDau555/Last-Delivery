using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class WorldManager : SingletonPersistent<WorldManager>, ISaveable
{
    #region KEYS & NAME
    private const string CURRENT_MONEY = "CurrentMoney";
    private const string CURRENT_DAY = "CurrentDay";
    private const string CURRENT_COLLECTED = "CurrentCollected";
    private const string MONEY = "Money Count";
    private const string DAYS = "Day Count";
    private const string COLLECTED = "Collected Count";
    private const string IS_OPEN_LV2 = "Is Open Lv2";
    #endregion
    
    #region Parameters
    [SerializeField] public int _money;
    [SerializeField] private int _currentDay;
    [SerializeField] private int _itemLostCount;
    private int _minMoneyToNextDay = 50; // Số tiền tối thiểu (mặc định là 50)
    private const int k_baseAmount = 50;
    private const float k_growRate = 1.25f;
    private int currentLevel;

    [Space(10)]
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI currentDayText;
    [SerializeField] private TextMeshProUGUI currentMoneyText;
    [SerializeField] private TextMeshProUGUI minMoneyToNextDayText;
    [SerializeField] private GameObject lostObjectUI;
    [SerializeField] private TextMeshProUGUI itemLostCountText;
    [SerializeField] private TextMeshProUGUI totalItemCountText;

    private PlayerController player;
    private CatAgent catAgent;

    public bool isOpenLv2 { get; private set; }
    #endregion

    #region Execute
    public override void Awake()
    {
        base.Awake();

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        catAgent = GameObject.Find("Cat Agent").GetComponent<CatAgent>();
        
        RefreshUI();
    }
    
    void Start()
    {
    }

    void Update()
    {

    }

    void OnEnable()
    {
        //SceneManager.sceneLoaded += OnSceneLoaded;
        DeliveryManager.Instance.OnDeliverySuccess += OnDeliverySuccess_AddMoney;
        DeliveryManager.Instance.OnDeliveryFail += OnDeliveryFail_PenaltyMoney;
    }


    void OnDisable()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;

        if (DeliveryManager.Instance != null)
        {
            DeliveryManager.Instance.OnDeliverySuccess -= OnDeliverySuccess_AddMoney;
            DeliveryManager.Instance.OnDeliveryFail -= OnDeliveryFail_PenaltyMoney;
        }
    }
    
    // private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    // {
    //     if (DeliveryManager.Instance != null)
    //     {
    //         DeliveryManager.Instance.OnDeliverySuccess += OnDeliverySuccess_AddMoney;
    //         DeliveryManager.Instance.OnDeliveryFail += OnDeliveryFail_PenaltyMoney;
    //     }
    // }
    #endregion

    #region Update World
    /// <summary>
    /// Event Add money
    /// </summary>
    private void OnDeliverySuccess_AddMoney(object sender, DeliveryManager.OnDeliveryEventArgs e)
    {
        CargoObjectSO cargoSO = e.CargoObjectSO;

        _money += cargoSO.cargoPrice;
        UpdateMoneyUI();
    }

    private void OnDeliveryFail_PenaltyMoney(object sender, DeliveryManager.OnDeliveryEventArgs e)
    {
        CargoObjectSO cargoSO = e.CargoObjectSO;

        _money -= cargoSO.penaltyPrice;
        _money = Mathf.Max(0, _money); // To prevent negative number 
        UpdateMoneyUI();
    }    
    
    /// <summary>
    /// Collect enough amount off money
    /// </summary>
    public bool TryNextDay()
    {
        if(_money >= _minMoneyToNextDay)
        {
            _currentDay++;
            _itemLostCount = 0;
           SpendMoney(_minMoneyToNextDay);

            Debug.Log(catAgent.currentMoodBar);

            player._currentHealth += 100;
            catAgent.currentMoodBar += 100;
            catAgent.UpdateMoodBar();

            UpdateMoneyUI();
            UpdateDayUI();
            UpdateLostUI();
            RefreshUI();

            SaveManager.Instance.SaveCheckpoint();

            _minMoneyToNextDay = Mathf.RoundToInt(k_baseAmount * Mathf.Pow(k_growRate, _currentDay - 1));
            
            return true;
        }
        return false;
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
    
    public void IncreaseLostItemFound()
    {
        if(SceneManager.GetActiveScene().name == "Lv2")
        {
            _itemLostCount++;
            UpdateLostUI();
        }
    }
    #endregion

    #region Save and Load
    public void Save(SaveData data)
    {
        data.Set(CURRENT_DAY, _currentDay);
        data.Set(CURRENT_MONEY, _money);
        data.Set(CURRENT_COLLECTED, _itemLostCount);
        data.Set(IS_OPEN_LV2, isOpenLv2);
    }

    public void Load(SaveData data)
    {
        _currentDay = data.Get<int>(CURRENT_DAY, this._currentDay);
        _money = data.Get<int>(CURRENT_MONEY, this._money);
        _itemLostCount = data.Get<int>(CURRENT_COLLECTED, this._itemLostCount);
        isOpenLv2 = data.Get<bool>(IS_OPEN_LV2, this.isOpenLv2);

        RefreshUI();
    }
    #endregion

    #region UI
    /// <summary>
    /// Refresh money text and day text when player load save file
    /// </summary>
    private void RefreshUI()
    {

        if (currentDayText == null || currentMoneyText == null)
        {
            currentDayText = GameObject.Find(DAYS)?.GetComponent<TextMeshProUGUI>();
            currentMoneyText = GameObject.Find(MONEY)?.GetComponent<TextMeshProUGUI>();
        }

        UpdateDayUI();
        UpdateMoneyUI();
        UpdateLostUI();
        
        if (_currentDay < 7)
        {
            Hide();
        }
        else
        {
            Show();
            isOpenLv2 = true;
        }
    }
    
    private void UpdateMoneyUI()
    {
        if (currentMoneyText != null)
            currentMoneyText.text = _money.ToString() + "/" + _minMoneyToNextDay.ToString();
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
            totalItemCountText.text = "/" + DeliveryManager.Instance.totalLostItemCount.ToString();
    }

    private void Show() => lostObjectUI?.SetActive(true);
    private void Hide() => lostObjectUI?.SetActive(false);
    #endregion
}