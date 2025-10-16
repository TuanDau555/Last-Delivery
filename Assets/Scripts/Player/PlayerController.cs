using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour, IObjectParent
{
    #region Parameter
    [Space(10)]
    [Header("Reference")]
    [SerializeField] private CharacterStatsSO playerStatsSO;

    [Space(10)]
    [Header("Required Component")]
    [SerializeField] private Transform playerCamera;

    [Space(10)]
    [Header("Hold Point")]
    [SerializeField] private Transform holdPoint;

    [Space(10)]
    [Header("Check ground")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundRadius = 0.4f;
    [SerializeField] private LayerMask groundMask;

    [Space(10)]
    [Header("Player Last Position")]
    [SerializeField]
    [Range(1f, 5f)]
    // The time that player's latest position last
    private float historicalPositionDuration = 1f;

    [SerializeField]
    [Range(0.001f, 1f)]
    // The time between update player's latest position  
    private float historicalPositionInterval = 0.1f;

    private float lastPositionTime;
    private int maxQueueSize;

    // Store all position in a Queue
    private Queue<Vector3> historicalVelocities;

    private Vector3 averageVelocity;
    #endregion

    private CharacterController _controller;
    private InputManager _inputManager;
    private CargoObject cargoObject;

    private float _clampAngle;
    private float _timer;
    private float _defaultPosY = 0; // starting point
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

        maxQueueSize = Mathf.CeilToInt(1f / historicalPositionInterval * historicalPositionDuration);
        historicalVelocities = new Queue<Vector3>(maxQueueSize);
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

        if (_inputManager.IsCrouch())
            HandleCrouch();

        HandleHeadBob();
        ApplyFinalMovement();

        UpdateHistoricalPosition();
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

    private void HandleCrouch()
    {
        if (IsGround() && !isTransition)
        {
            StartCoroutine(CrouchStand());
        }
    }

    private void HandleHeadBob()
    {
        if (!IsGround()) return;

        float _walkBobAmount = playerStatsSO.stats.walkBobAmount;
        float _sprintBobAmount = playerStatsSO.stats.sprintBobAmount;
        float _crouchBobAmount = playerStatsSO.stats.crouchBobAmount;

        float _walk = playerStatsSO.stats.walkBobSpeed;
        float _sprint = playerStatsSO.stats.sprintBobSpeed;
        float _crouch = playerStatsSO.stats.crouchBobSpeed;

        HeadBobbing(_walk, _crouch, _sprint, _walkBobAmount, _crouchBobAmount, _sprintBobAmount);
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

    private IEnumerator CrouchStand()
    {

        // prevent standing if under something 
        if (!isCrouching && Physics.Raycast(playerCamera.transform.position, Vector3.up, 1))
        {
            yield break;
        }

        // flag is changing state
        isTransition = true;

        float timeElapsed = 0;
        float timeToCrouch = playerStatsSO.stats.timeToCrouch;

        // Storing current height and center of Player
        float currentHeight = _controller.height;
        Vector3 currentCenter = _controller.center;

        // Switch state
        isCrouching = !isCrouching;

        // Determine the height and center point of Player  
        float targetHeight = isCrouching ? playerStatsSO.stats.standHeight : playerStatsSO.stats.crouchHeight;
        Vector3 targetCenter = isCrouching ? playerStatsSO.stats.standingCenter : playerStatsSO.stats.standingCenter;

        // Execute crouch
        while (timeElapsed < timeToCrouch)
        {
            _controller.height = Mathf.Lerp(currentHeight, targetHeight, timeElapsed / timeToCrouch);
            _controller.center = Vector3.Lerp(currentCenter, targetCenter, timeElapsed / timeToCrouch);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        // Ensure that the value this correct
        _controller.height = targetHeight;
        _controller.center = targetCenter;

        // End changing State
        isTransition = false;
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

    private void HeadBobbing(float walk, float crouch, float sprint, float walkBobAmount, float crouchBobAmount, float sprintBobAmount)
    {
        float headBobSpeed = !isCrouching ? crouch : _inputManager.IsSprinting() ? sprint : walk;
        float headBobAmount = !isCrouching ? crouchBobAmount : _inputManager.IsSprinting() ? sprintBobAmount : walkBobAmount;

        if (Mathf.Abs(_moveDirection.x) > 0.1f || Mathf.Abs(_moveDirection.z) > 0.1f)
        {
            _timer += Time.deltaTime * headBobSpeed;
            // just want to bobbing on Y axis
            playerCamera.transform.localPosition = new Vector3(
                playerCamera.transform.localPosition.x,
                _defaultPosY + Mathf.Sin(_timer) * headBobAmount,
                playerCamera.transform.localPosition.z
            );
        }
    }
    private bool IsGround() => Physics.CheckSphere(groundCheck.position, groundRadius, groundMask);
    #endregion

    #region Historical Position
    private void UpdateHistoricalPosition()
    {
        // Only add player's velocities every certain amount of time to avoid updating too frequent  
        if (lastPositionTime + historicalPositionInterval <= Time.time)
        {
            // if queue is ful of player's velocities...
            if (historicalVelocities.Count == maxQueueSize)
            {
                //... Delete old one
                historicalVelocities.Dequeue();
            }

            //... And add new one
            historicalVelocities.Enqueue(_controller.velocity);

            lastPositionTime = Time.time;
        }
    }
    
    /// <summary>
    /// Calculates the average horizontal (XZ) velocity from the recorded historicalVelocities.
    /// Ignores the vertical (Y) component and returns Vector3.zero if there are no samples.
    /// </summary>
    /// <returns>Average horizontal velocity as a Vector3 (Y = 0).</returns>
    public Vector3 GetAverageVelocity()
    {
        // Prevent null and division by 0
        if (historicalVelocities == null || historicalVelocities.Count == 0)
            return Vector3.zero;

        averageVelocity = Vector3.zero;
        foreach (Vector3 velocity in historicalVelocities)
        {
            averageVelocity += velocity;
        }
        averageVelocity.y = 0;
        return averageVelocity / historicalVelocities.Count;
    }
    #endregion
    
    #region Object Parent
    public Transform GetObjectFollowTransform()
    {
        return holdPoint;
    }

    public void SetCargoObject(CargoObject cargoObject)
    {
        if (this.cargoObject != null)
        {
            Destroy(this.cargoObject.gameObject);
        }
        this.cargoObject = cargoObject;
    }

    public CargoObject GetCargoObject()
    {
        return cargoObject;
    }

    public void ClearCargoObject()
    {
        cargoObject = null;
    }

    public bool HasCargoObject()
    {
        return cargoObject != null;
    }
    #endregion
}
