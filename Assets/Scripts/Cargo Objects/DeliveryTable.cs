using UnityEngine;

public class DeliveryTable : BaseInteract
{
    public CargoObjectSO _expectedCargo { get; private set; }


    public void SetExpectedTable(CargoObjectSO cargoObjectSO)
    {
        _expectedCargo = cargoObjectSO;
    }
    
    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);
        // TODO: Destroy the object when player delivery and check if it correct or not 
        if (playerController.HasCargoObject())
        {
            CargoObject cargoObject = playerController.GetCargoObject();
            CargoObjectSO cargoObjectSO = cargoObject.GetCargoObjectSO();

            if (DeliveryManager.Instance.CheckDeliveryOrder(cargoObjectSO, this))
            {
                cargoObject.DestroySelf();
                Debug.Log("Delivery Success");
            }
            else
            {
                Debug.LogWarning("Wrong Delivery expected order is: " + _expectedCargo);
            }
        }
    }
}
