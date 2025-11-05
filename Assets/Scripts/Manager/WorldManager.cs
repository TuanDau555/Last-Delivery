using System;
using TMPro;
using UnityEngine;

public class WorldManager : SingletonPersistent<WorldManager>, ISaveable
{
    // <--- **Đã thêm:** Sự kiện Game Over TĨNH
    public static event Action OnGameOver; 
    
    #region KEYS & NAME
    private const string CURRENT_MONEY = "CurrentMoney";
    private const string CURRENT_DAY = "CurrentDay";
    private const string MONEY = "Money Count";
    private const string DAYS = "Day Count";
    #endregion
    
    #region Parameters
    [SerializeField] public int _money;
    [SerializeField] private int _currentDay;
    private int currentLevel;

    [Space(10)]
    [Header("Game Rules")]
    [SerializeField] private int _minMoneyToNextDay = 50; // Số tiền tối thiểu
    private bool isGameOver = false; // <--- Biến theo dõi trạng thái Game Over
    
    [Space(10)]
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI currentDayText;
    [SerializeField] private TextMeshProUGUI currentMoneyText;
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
        if (isGameOver) return; // Nếu Game Over rồi thì không làm gì nữa
        
        // Kiểm tra số tiền tối thiểu
        if (_money >= _minMoneyToNextDay)
        {
            _currentDay++;
            currentDayText.text = _currentDay.ToString();
            Debug.Log($"Passed to Day {_currentDay}");
        }
        else
        {
            Debug.LogWarning("Not enough money to pass to the next day!");
            GameOver(); // Gọi Game Over
        }
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
    
    /// <summary>
    /// Handle Game Over state
    /// </summary>
    public void GameOver()
    {
        if (isGameOver) return; // Chỉ thực hiện Game Over một lần
        
        isGameOver = true;
        Time.timeScale = 0; // Dừng game
        Debug.Log("<color=red>GAME OVER! (WorldManager)</color>");
        
        // Kích hoạt sự kiện Game Over
        OnGameOver?.Invoke(); 
        
        // TODO: Logic hiển thị màn hình Game Over UI
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
         if (currentDayText == null || currentMoneyText == null)
        {
            currentDayText = GameObject.Find(DAYS)?.GetComponent<TextMeshProUGUI>();
            currentMoneyText = GameObject.Find(MONEY)?.GetComponent<TextMeshProUGUI>();
        }
        
        if (currentDayText != null)
            currentDayText.text = _currentDay.ToString();
        if (currentMoneyText != null)
            currentMoneyText.text = _money.ToString();
    }
    #endregion
}