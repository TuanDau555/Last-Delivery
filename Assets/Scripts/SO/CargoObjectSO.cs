using UnityEngine;

[CreateAssetMenu(fileName = "Objects", menuName = "Objects/Cargo Object")]
public class CargoObjectSO : ScriptableObject
{
    public string id;
    public string objectName;

    [Tooltip("Lost object is only use in Lv2 Scene")]
    public bool isLostItem;
    [Tooltip("The object when player take an order")]
    public Transform cargoOrderPrefab;
    [Tooltip("The object to delivery")]
    public Transform cargoPrefab;
    [Tooltip("This is where AI leads the player to")]
    public Transform destination;
    [Tooltip("Object image to present it")]
    public Sprite cargoOrderSprite;
}
