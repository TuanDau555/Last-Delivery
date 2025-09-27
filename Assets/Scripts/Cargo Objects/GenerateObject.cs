using UnityEngine;

public class GenerateObject : BaseInteract
{
    [SerializeField] private CargoObjectSO cargoObjectSO;

    public override void Interact(PlayerController playerController)
    {
        if (!playerController.HasCargoObject())
        {
            // Instantiate object...
            Transform cargoObjectTransform = Instantiate(cargoObjectSO.objectPrefab);
            cargoObjectTransform.localPosition = Vector3.zero;

            //... And give it to player
            cargoObjectTransform.GetComponent<CargoObject>().SetObjectParent(playerController);
        }
    }
}
