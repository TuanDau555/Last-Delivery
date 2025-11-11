using UnityEngine;

public class LostCargoObject : BaseInteract
{
    [SerializeField] private CargoObjectSO cargoObjectSO;
    
    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);
        if (!playerController.HasCargoObject())
        {
            DeliveryManager.Instance.AddLostItemToWaitingList(cargoObjectSO);
            DeliveryManager.Instance.TableToDelivery(cargoObjectSO);
            CargoObject.SpawnCargoObject(cargoObjectSO, playerController);

            UIManager.Instance.ShowDeliveryTableFeedback(DeliveryManager.Instance.TableToDelivery(cargoObjectSO).name);
            
            Destroy(transform.parent.gameObject);
        }
        else
        {
            UIManager.Instance.ShowPlayerHoldSomethingFeedback();
        }
    }
}