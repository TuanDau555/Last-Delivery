using UnityEngine;

public class ChangeSceneDoor : BaseInteract
{
    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);
        if (!playerController.HasCargoObject())
        {
            if (WorldManager.Instance.isOpenLv2)
            {
                LoadSceneManager.Instance.StartChangeScene();
            }
            else
            {
                UIManager.Instance.ShowChangeConditionFeedback();
            }
        }
        else
        {
            UIManager.Instance.FinishTheOrderFeedback();
        }
    }
}