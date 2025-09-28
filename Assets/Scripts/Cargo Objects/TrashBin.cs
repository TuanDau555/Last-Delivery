using UnityEngine;

public class TrashBin : BaseInteract
{
    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);

        if (playerController.HasCargoObject())
        {
            playerController.GetCargoObject().DestroySelf();
            Debug.Log($"You have threw into trash bin");
        }

        else
        {
            Debug.LogWarning("Nothing to throw away");
        }
    }
}
