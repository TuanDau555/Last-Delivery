using UnityEngine;

public class BaseInteract : MonoBehaviour, IObjectParent
{

    [SerializeField] private Transform placePoint;

    private CargoObject _cargoObject;

    public virtual void Interact(PlayerController playerController){}

    public void SetCargoObject(CargoObject cargoObject)
    {
        this._cargoObject = cargoObject;
    }

    public CargoObject GetCargoObject() => _cargoObject;

    public Transform GetObjectFollowTransform() => placePoint;

    public bool HasCargoObject() => _cargoObject != null;

    public void ClearCargoObject() => _cargoObject = null;
}
