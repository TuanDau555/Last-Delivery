using UnityEngine;

public class GenerateObject : BaseInteract
{
    [SerializeField] private CargoObjectSO cargoObjectSO;

    public override void Interact(PlayerController playerController)
    {
        if (!playerController.HasCargoObject())
        {
            CargoObject.SpawnCargoObject(cargoObjectSO, playerController);
        }
        else
        {
            Debug.LogWarning("You already hold something");
        }
    }
}
