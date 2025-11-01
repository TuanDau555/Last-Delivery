public class ChangeSceneDoor : BaseInteract
{
    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);
        LoadSceneManager.Instance.StartChangeScene();
    }
}