using System;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    #region Parameter 
    [Header("Reference")]
    [SerializeField] private CharacterStatsSO playerStatsSO;

    [Header("Required Component")]
    [SerializeField] private Transform playerCamera;


    [Header("Check ground")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.4f;
    [SerializeField] private LayerMask groundMask;
    #endregion

    private CharacterController _controller;
    private InputManager _inputManager;

    private float _clampAngle;
    private Vector3 _playerVelocity;
    private Vector3 _moveDirection;
    private Vector2 _moveInput;
    private Vector3 _mouseDirection;
    private Vector2 _mouseDelta;

    // I set it true at init because...
    // ...I want to make the player to be able crouching immediately at the beginning of the game
    private bool isCrouching = true;
    private bool isTransition;

    #region Execute
    void Awake()
    {
        _controller = GetComponent<CharacterController>();

        // Don't need to show player cursor cause this is FPS game
        // We use cross hair instead
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (_mouseDirection == null)
        {
            _mouseDirection = transform.localRotation.eulerAngles;
        }
    }

    void Start()
    {
        _inputManager = InputManager.Instance;
        playerCamera = Camera.main.transform;
    }

    void Update()
    {
        HandleMovementInput();
        HandleMouseLook();

        ApplyFinalMovement();
    }

    #endregion

    #region Get Input 
    private void HandleMovementInput()
    {
        // Get current Input
        _moveInput = _inputManager.GetPlayerMovement();

        // Convert input to movement direction
        _moveDirection = new Vector3(_moveInput.x, 0, _moveInput.y).normalized;
    }
    private void HandleMouseLook()
    {
        _clampAngle = playerStatsSO.stats.lookLimit;

        // Get mouse Delta
        _mouseDelta = _inputManager.GetMouseDelta();
        ApplyFinalLook(_clampAngle);
    }
    #endregion

    #region Final Calculate        
    private void ApplyFinalMovement()
    {
        float walk = playerStatsSO.stats.walkSpeed;
        float sprint = playerStatsSO.stats.sprintSpeed;
        float crouch = playerStatsSO.stats.crouchSpeed;

        if (IsGround() && _playerVelocity.y < 0)
        {
            // reset Y velocity
            _playerVelocity.y = -2f;
        }

        // Apply gravity
        _playerVelocity.y += playerStatsSO.stats.gravity * Time.deltaTime;

        // if crouch using crouchSpeed else just use walk speed
        // NOTE: we don't want to sprint when crouch
        float finalSpeed = !isCrouching ? crouch : _inputManager.IsSprinting() ? sprint : walk;

        // Apply movement
        Vector3 finalMove = (_moveDirection * finalSpeed) + (_playerVelocity.y * Vector3.up);
        _controller.Move(finalMove * Time.deltaTime);
    }

    private void ApplyFinalLook(float clampAngle)
    {
        // Look Sensitive
        _mouseDirection.x += _mouseDelta.x * playerStatsSO.stats.lookSensitive * Time.deltaTime;
        _mouseDirection.y += _mouseDelta.y * playerStatsSO.stats.lookSensitive * Time.deltaTime;

        // Limit look angle
        _mouseDirection.y = Mathf.Clamp(_mouseDirection.y, -clampAngle, clampAngle);

        // Apply Rotation
        transform.localRotation = Quaternion.Euler(-_mouseDirection.y, _mouseDirection.x, 0f);

        // Player direction follow camera look
        _moveDirection = playerCamera.TransformDirection(_moveDirection);
    }

    #endregion
    private bool IsGround() => Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
}
