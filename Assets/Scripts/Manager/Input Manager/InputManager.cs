using UnityEngine;

public class InputManager : Singleton<InputManager>
{
    private PlayerInput playerInput;

    public override void Awake()
    {
        base.Awake();
        playerInput = new PlayerInput();
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        playerInput.Disable();
    }

    public Vector2 GetPlayerMovement() => playerInput.Player.Moving.ReadValue<Vector2>();
    public Vector2 GetMouseDelta() => playerInput.Player.Look.ReadValue<Vector2>();
    public bool IsSprinting() => playerInput.Player.Sprint.ReadValue<float>() > 0f; // Player have to move to Sprint
    public bool IsCrouch() => playerInput.Player.Crouch.WasPressedThisFrame(); 
}
