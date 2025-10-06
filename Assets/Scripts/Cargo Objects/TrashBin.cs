using UnityEngine;

public class TrashBin : BaseInteract
{
    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);

        if (playerController.HasCargoObject())
        {
            CargoObject cargoObject = playerController.GetCargoObject();
            CargoObjectSO cargoSO = cargoObject.GetCargoObjectSO();

            DeliveryManager.Instance.RemoveOrder(cargoSO);
            DeliveryManager.Instance.ClearDeliveryObject();

            
            cargoObject.DestroySelf();
            Debug.Log($"You have threw into trash bin");
        }

        else
        {
            Debug.LogWarning("Nothing to throw away");
        }
    }
}
