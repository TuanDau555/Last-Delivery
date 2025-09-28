using UnityEngine;

public class CatAgent : BaseInteract
{
    public Transform target;

    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);

        // TODO: Attach the destination in CargoObjectSO to this so we can activate NavMesh Agent 
        if (playerController.HasCargoObject())
        {
            CargoObjectSO cargoObjectSO = playerController.GetCargoObject().GetCargoObjectSO();
            DeliveryTable table = DeliveryManager.Instance.TableToDelivery(cargoObjectSO);

            target = table.transform;
            Debug.Log("Object to delivery: " + cargoObjectSO);
            Debug.Log("Location to get the deliver: " + table.name);
        }
        else
        {
            Debug.Log("you have nothing to do");
        }
    }
}
