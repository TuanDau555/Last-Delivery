using UnityEngine;

public class Bed : BaseInteract
{
    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);

        if (WorldManager.Instance.TryNextDay())
        {
            UIManager.Instance.NextDayWelcome();
            // TODO: Fade in/out
        }
        else
        {
            UIManager.Instance.NotEnoughMoney();
        }
    }
}