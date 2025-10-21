using System;
using UnityEngine;

[CreateAssetMenu(fileName = "Objects", menuName = "Objects/Shop Item")]
public class ShopItemSO : ScriptableObject
{
    [Header("UI")]
    public string itemName;
    public GameObject displayVisual;
    [Range(10, 100)]
    public int price;

    [TextArea]
    [Tooltip("Details of the object")]
    public string description;

    [Space(10)]
    [Header("Gameplay")]
    public bool isCatItem;
    public bool isUpgradeItem;
    
    [Tooltip("Max Stats Upgrade")]
    [Range(1f, 100f)]
    public float upgradeValue;

    [Range(0f, 50f)]
    public float catMoodBuff;
    [Range(0f, 50f)]
    public float playerSpeedBuff;
    [Range(0f, 50f)]
    public float playerHPBuff;

    [Tooltip("buff expiration time")]
    [Range(0f, 360f)]
    public float buffTime;
}