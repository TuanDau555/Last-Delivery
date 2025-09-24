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

    }

    void Update()
    {
        HandleInteraction();
    }

    void OnDisable()
    {

    }
    #endregion

    #region Interact
    private void HandleInteraction()
    {
        Vector3 lookDirection = transform.forward; // Ray cast shoot direction
        Vector3 playerPosition = transform.position; // Ray cast's start shooting position

        // Shoot the Raycast to detect is pointing object
        // Begin at shoot from player, direction to shoot is look direction
        if (Physics.Raycast(playerPosition, lookDirection, interactDistance, interactLayerMask))
        {
            Debug.Log("Is interact:");
        }
    }
    #endregion
}
