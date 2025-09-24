using System;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Interactable : Singleton<Interactable>
{
    #region Parameter
    [Range(1, 5)]
    [SerializeField] private float interactDistance = 2f;
    [SerializeField] private LayerMask interactLayerMask;
    private BaseInteract selectedObject;
    #endregion

    #region Execute
    void Start()
    {
        InputManager.Instance.OnInteractAction += InputManager_OnInteractAction;
    }

    void Update()
    {
        HandleInteraction();
    }

    void OnDisable()
    {
        InputManager.Instance.OnInteractAction -= InputManager_OnInteractAction;
    }
    #endregion

    #region Interact Events

    public event EventHandler<OnSelectedChangedEventArgs> OnSelectedChanged;
    public class OnSelectedChangedEventArgs : EventArgs
    {
        public BaseInteract selectedObjectEvent;
    }

    private void InputManager_OnInteractAction(object sender, EventArgs e)
    {
        if (selectedObject != null)
        {
            selectedObject.Interact(this.GetComponent<PlayerController>());
        }
    }
    #endregion

    #region Interact
    private void HandleInteraction()
    {
        Vector3 lookDirection = transform.forward; // Raycast's shoot direction
        Vector3 playerPosition = transform.position; // Raycast's start shooting position

        // Shoot the Raycast to detect is pointing object
        // Begin at shoot from player, direction to shoot is look direction
        if (Physics.Raycast(playerPosition, lookDirection, out RaycastHit raycastHit, interactDistance, interactLayerMask))
        {
            if (raycastHit.transform.TryGetComponent(out BaseInteract baseInteract))
            {
                // Is interact
                if (baseInteract != selectedObject)
                {
                    SetSelectedObject(baseInteract);
                }
            }
            else
            {
                SetSelectedObject(null);
            }
        }
        else
        {
            // Is not looking at the interact object
            SetSelectedObject(null);
        }
    }

    // When Selected The Object
    private void SetSelectedObject(BaseInteract selectedObject)
    {
        this.selectedObject = selectedObject;
        OnSelectedChanged?.Invoke(this, new OnSelectedChangedEventArgs
        {
            selectedObjectEvent = selectedObject
        });
    }
    #endregion
}
