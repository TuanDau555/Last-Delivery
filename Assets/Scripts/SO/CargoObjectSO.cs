using UnityEngine;

[CreateAssetMenu(fileName = "Objects", menuName = "Objects/Cargo Object")]
public class CargoObjectSO : ScriptableObject
{
    public string id;
    public string objectName;
    public Transform objectPrefab;
    [Tooltip("Object image to present it")]
    public Sprite sprite;
}
