using UnityEngine;

public class BaseInteract : MonoBehaviour, IObjectParent
{

    [SerializeField] private Transform placePoint;

    private CargoObject cargoObject;

    public virtual void Interact(PlayerController playerController){}

    public CargoObject GetCargoObject()
    {
        return cargoObject;
    }

    public Transform GetObjectFollowTransform()
    {
        return placePoint;
    }

    public void SetCargoObject(CargoObject cargoObject)
    {
        this.cargoObject = cargoObject;
    }

    public bool HasCargoObject()
    {
        return cargoObject != null;
    }

    public void ClearCargoObject()
    {
        cargoObject = null;
    }
}
