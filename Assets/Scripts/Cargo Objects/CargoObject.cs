using UnityEngine;

/// <summary>
/// Cargo objects are items that are being shipped and obtained from the player.
/// It can be placed on something, held by player. 
/// </summary>
public class CargoObject : MonoBehaviour
{
    private CargoObjectSO cargoObjectSO;
    private IObjectParent objectParent;

    public IObjectParent GetObjectParent() => objectParent;
    public CargoObjectSO GetCargoObjectSO() => cargoObjectSO;

    public void SetObjectParent(IObjectParent objectParent)
    {
        // There is no object on the table and player is holding nothing. 
        if (this.objectParent != null)
        {
            this.objectParent.ClearCargoObject();
        }

        this.objectParent = objectParent; // the item is now belong to the player or table

        if (objectParent.HasCargoObject())
        {
            Debug.LogWarning($"{objectParent} already has a object assigned. Cannot assign a new one");
            return;
        }

        // else
        objectParent.SetCargoObject(this); // Set the item parent to where it belong

        transform.parent = objectParent.GetObjectFollowTransform(); // just where it is being place at
        transform.localPosition = Vector3.zero; // reset it
        transform.localRotation = Quaternion.identity; // reset it
    }

    public static CargoObject SpawnCargoObject(CargoObjectSO _cargoObjectSO, IObjectParent _objectParent)
    {
        Transform _cargoObjectTransform = Instantiate(_cargoObjectSO.cargoPrefab);

        CargoObject cargoObject = _cargoObjectTransform.GetComponent<CargoObject>();
        cargoObject.cargoObjectSO = _cargoObjectSO;
        cargoObject.SetObjectParent(_objectParent);
        return cargoObject;
    }
    
    public void SetCargoObjectSO(CargoObjectSO so)
    {
        cargoObjectSO = so;
    }
    
    public void DestroySelf()
    {
        objectParent.ClearCargoObject();
        Destroy(gameObject);
    }
}
