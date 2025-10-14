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

            // Just want to call this method if it delivery object
            if(DeliveryManager.Instance.currentDeliveryObject == cargoSO)
            {
                Debug.Log($"You have throw {DeliveryManager.Instance.currentDeliveryObject}");
                DeliveryManager.Instance.ClearDeliveryObject();
            }

            cargoObject.DestroySelf();
            Debug.Log($"You have threw into trash bin");

            // Cat only stop when there NO order to delivery
            if (!DeliveryManager.Instance.HasPendingOrder())
            {
                DeliveryManager.Instance.TriggerStopDelivery();
                Debug.Log("There are no object in List cat stop");
            }
        }

        else
        {
            Debug.LogWarning("Nothing to throw away");
        }
    }
}
