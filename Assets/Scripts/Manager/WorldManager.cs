using System;
using TMPro;
using UnityEngine;

public class WorldManager : Singleton<WorldManager>
{
    #region Parameters
    [SerializeField] public int _money;
    [SerializeField] private int _currentDay;

    [Space(10)]
    [Header("UI")]
    [SerializeField] private TextMeshProUGUI currentDayText;
    [SerializeField] private TextMeshProUGUI currentMoneyText;
    #endregion

    #region Execute
    void Start()
    {        
        currentDayText.text = _currentDay.ToString();
        currentMoneyText.text = _money.ToString();
        DeliveryManager.Instance.OnDeliverySuccess += AddMoney;
    }

    void Update()
    {

    }

    void OnDisable()
    {
        DeliveryManager.Instance.OnDeliverySuccess -= AddMoney;        
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
    
    #region Events
    
    #endregion
}