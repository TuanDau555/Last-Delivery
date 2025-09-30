using UnityEngine;

public class OrderObjects : BaseInteract
{
    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);

        // Get Order
        CargoObjectSO order = DeliveryManager.Instance.AddOrder();

        // Give the prefabs instruction for player
        Transform cargoOrder = Instantiate(order.cargoOrderPrefab);
        CargoObject cargo = cargoOrder.GetComponent<CargoObject>();
        cargo.SetCargoObjectSO(order);
        cargo.SetObjectParent(playerController);

        DeliveryManager.Instance.SetCurrentDeliveryObject(order);  
        Debug.Log("You have an order to delivery");
    }
}
