using System;
using UnityEngine;

public class InputManager : SingletonPersistent<InputManager>
{
    private PlayerInput playerInput;

    // Events
    public event EventHandler OnInteractAction;

    public override void Awake()
    {
        base.Awake();
        playerInput = new PlayerInput();
        playerInput.Player.Interact.performed += Interact_Performed;
    }

    void OnEnable()
    {
        playerInput.Enable();
    }

    void OnDisable()
    {
        //playerInput.Disable();
    }

    public Vector2 GetPlayerMovement() => playerInput.Player.Moving.ReadValue<Vector2>();
    public Vector2 GetMouseDelta() => playerInput.Player.Look.ReadValue<Vector2>();
    public bool IsSprinting() => playerInput.Player.Sprint.ReadValue<float>() > 0f; // Player have to move to Sprint
    public bool IsCrouch() => playerInput.Player.Crouch.WasPressedThisFrame();
    public bool IsOpenFlashLight() => playerInput.Player.FlashLight.WasPressedThisFrame();
    public bool IsPause() => playerInput.Player.PauseGame.WasPressedThisFrame();
    private void Interact_Performed(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        OnInteractAction?.Invoke(this, EventArgs.Empty);
    }
}
