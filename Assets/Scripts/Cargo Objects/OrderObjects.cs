using UnityEngine;

public class OrderObjects : BaseInteract
{
    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);


        // Give the prefabs instruction for player
        if (!playerController.HasCargoObject())
        {
            // Get Order
            CargoObjectSO order = DeliveryManager.Instance.AddRandomOrder();
        
            Transform cargoOrder = Instantiate(order.cargoOrderPrefab);
            CargoObject cargo = cargoOrder.GetComponent<CargoObject>();
        
            cargo.SetObjectParent(playerController);
            cargo.SetCargoObjectSO(order);
        
            DeliveryManager.Instance.SetCurrentDeliveryObject(order);
            Debug.Log("You have an order to delivery");
        }
        else
        {
            Debug.LogWarning("You already hold something");
        }

    }
}
