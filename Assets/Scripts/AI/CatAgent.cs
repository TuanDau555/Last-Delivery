using UnityEngine;
using UnityEngine.AI;

public class CatAgent : BaseInteract
{
    [Space(10)]
    [SerializeField] private Transform playerPosition;

    [Range(1, 10)]
    [SerializeField] private float followDistance;

    private NavMeshAgent _catAgent;


    #region Execute
    void Start()
    {
        _catAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        FollowPlayer();
    }
    #endregion

    #region Cat Follow

    // TODO: ONLY follow player when in specific STATE
    private void FollowPlayer()
    {
        // Get player Distance between player and cat
        float playerDistance = Vector3.Distance(transform.position, playerPosition.position);

        // If the player moves out of the cat's following range
        if (playerDistance > followDistance)
        {
            // Cat follow
            _catAgent.SetDestination(playerPosition.position);

        }
        else if (playerDistance <= _catAgent.stoppingDistance)
        {
            // cat just standing there
            _catAgent.ResetPath();
        }
    }
    #endregion

    #region Interact
    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);

        // TODO: Attach the destination in CargoObjectSO to this so we can activate NavMesh Agent 
        if (playerController.HasCargoObject())
        {
            CargoObjectSO cargoObjectSO = playerController.GetCargoObject().GetCargoObjectSO();

            Debug.Log("Object to delivery: " + cargoObjectSO);

            // For testing
            DeliveryTable table = DeliveryManager.Instance.TableToDelivery(cargoObjectSO);
            Debug.Log("Location to get the deliver: " + table.name);
        }
        else
        {
            Debug.Log("you have nothing to do");
        }
    }
    #endregion

    #region Draw Cat Distance
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, followDistance);
    }
    #endregion
}
