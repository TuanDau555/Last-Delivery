using UnityEngine;

public class Bed : BaseInteract
{
    [Space(10)]
    [SerializeField] private Animator sceneTransition;
    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);

        if (WorldManager.Instance.TryNextDay())
        {
            UIManager.Instance.NextDayWelcome();
            sceneTransition.SetTrigger("Bed Crossfade");
        }
        else
        {
            UIManager.Instance.NotEnoughMoney();
        }
    }
}