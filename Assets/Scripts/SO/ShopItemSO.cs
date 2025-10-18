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

    [Tooltip("Percentage of the buff applied")]
    [Range(1f, 50f)]
    public float buffValue;
}