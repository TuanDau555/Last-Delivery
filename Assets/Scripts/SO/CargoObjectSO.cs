using UnityEngine;

[CreateAssetMenu(fileName = "Objects", menuName = "Objects/Cargo Object")]
public class CargoObjectSO : ScriptableObject
{
    public string id;
    public string objectName;

    [Space(10)]
    [Header("Gameplay")]
    [Tooltip("Lost object is only use in Lv2 Scene")]
    public bool isLostItem;
    [Tooltip("Price of this item when earn")]
    [Range(10, 50)]
    public int cargoPrice;
    [Tooltip("Price of this item when deliver wrong order")]
    [Range(20, 45)]
    public int penaltyPrice;

    [Space(10)]
    [Header("UI")]
    [Tooltip("The object when player take an order")]
    public Transform cargoOrderPrefab;
    [Tooltip("The object to delivery")]
    public Transform cargoPrefab;
    [Tooltip("Object image to present it")]
    public Sprite cargoOrderSprite;
}
