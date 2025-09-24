using UnityEngine;

public interface IObjectParent
{
    public Transform GetObjectFollowTransform();
    public void SetCargoObject(CargoObject cargoObject);
    public CargoObject GetCargoObject();
    public void ClearCargoObject();
    public bool HasCargoObject();
}
