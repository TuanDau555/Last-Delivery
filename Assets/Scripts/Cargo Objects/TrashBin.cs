using UnityEngine;

public class TrashBin : BaseInteract
{
    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);

        if (!playerController.HasCargoObject())
        {
            Debug.LogWarning("Nothing to throw away");
            return;
        }

        CargoObject cargoObject = playerController.GetCargoObject();
        CargoObjectSO cargoSO = cargoObject.GetCargoObjectSO();

        // If is in Deliver and want to thrown out
        if (DeliveryManager.Instance.currentDeliveryState == DeliveryState.DELIVER)
        {
            Debug.Log($"[TrashBin] You threw away the current delivery object ({cargoSO.objectName}).");

            // ... Just remove from the current Deliver object 
            DeliveryManager.Instance.ClearDeliveryObject();
        }
        else
        {
            if (DeliveryManager.Instance.currentDeliveryObject == cargoSO)
            {
                Debug.Log($"[TrashBin] You threw away a waiting order ({cargoSO.objectName}).");
                DeliveryManager.Instance.RemoveOrder(cargoSO);
            }
        }

        // Destroy object and Remove from player
        cargoObject.DestroySelf();
        playerController.ClearCargoObject();

        // If there is no waiting object in the list, Stop the cat
        if (!DeliveryManager.Instance.HasPendingOrder())
        {
            Debug.Log("[TrashBin] No pending order left, cat stops following.");
            DeliveryManager.Instance.TriggerStopDelivery();
        }
    }
}

