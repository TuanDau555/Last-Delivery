using UnityEngine;

public class ChangeSceneDoor : BaseInteract
{
    public override void Interact(PlayerController playerController)
    {
        if (!playerController.HasCargoObject())
        {
            base.Interact(playerController);
            LoadSceneManager.Instance.StartChangeScene();
        }
        else
        {
            Debug.LogWarning("Don't try to steal it!");
        }
    }
}