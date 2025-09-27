using UnityEngine;

public class BaseInteract : MonoBehaviour, IObjectParent
{

    [SerializeField] private Transform placePoint;

    private CargoObject cargoObject;

    public virtual void Interact(PlayerController playerController){}

    public void SetCargoObject(CargoObject cargoObject)
    {
        this.cargoObject = cargoObject;
    }

    public CargoObject GetCargoObject() => cargoObject;

    public Transform GetObjectFollowTransform() => placePoint;

    public bool HasCargoObject() => cargoObject != null;

    public void ClearCargoObject() => cargoObject = null;
}
