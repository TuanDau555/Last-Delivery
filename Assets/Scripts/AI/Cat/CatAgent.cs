using System;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class CatAgent : BaseInteract
{
    #region Parameter
    [Header("Pref")]
    [SerializeField] private CatStatsSO catStatsSO;
    [SerializeField] private NavMeshAgent catAgent;
    [SerializeField] private Transform playerTransform;

    [Space(10)]
    [Header("UI / Display")]
    [SerializeField] private SpriteRenderer orderDisplay; // Sprite on cat back

    private StateMachine _stateMachine;
    private bool _isDelivery;
    #endregion

    #region Execute
    void Start()
    {
        _stateMachine = new StateMachine();
        CatState(this, catAgent, _stateMachine);
        DeliveryManager.Instance.OnStartDelivery += Delivery_OnStartDelivery;
    }

    void OnDisable()
    {
        DeliveryManager.Instance.OnStartDelivery -= Delivery_OnStartDelivery;
    }

    void Update()
    {
        _stateMachine.Update();
    }

    void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }
    #endregion

    #region Interact
    public override void Interact(PlayerController playerController)
    {
        base.Interact(playerController);


        if (playerController.HasCargoObject())
        {
            CargoObjectSO cargoObjectSO = playerController.GetCargoObject().GetCargoObjectSO();
            DeliveryTable table = DeliveryManager.Instance.TableToDelivery(cargoObjectSO);

            Debug.Log("Object to delivery: " + cargoObjectSO);

            // Assign the sprite to cat
            if (orderDisplay != null && cargoObjectSO.cargoOrderSprite != null)
            {
                orderDisplay.sprite = cargoObjectSO.cargoOrderSprite;
                orderDisplay.enabled = true;

                // TODO: ADD start delivery Event here 
                DeliveryManager.Instance.TriggerStartDelivery(cargoObjectSO, table);
            }
            // For testing
            Debug.Log("Location to get the deliver: " + table.name);
        }
        else
        {
            Debug.Log("you have nothing to do");
        }
    }
    #endregion

    #region Add Transition
    void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);
    #endregion

    #region Cat State
    /// <summary>
    /// We add cat State here
    /// </summary>
    /// <param name="cat">this cat</param>
    /// <param name="agent">this Agent</param>
    void CatState(CatAgent cat, NavMeshAgent agent, StateMachine stateMachine)
    {

        var idleState = new CatIdleState(cat, agent);
        var followState = new FollowState(cat, agent, playerTransform, catStatsSO);

        Any(idleState, new FuncPredicate(() => !_isDelivery));
        At(idleState, followState, new FuncPredicate(() => _isDelivery));

        // Set Initial State
        stateMachine.SetState(idleState);
    }
    #endregion

    #region Events
    private void Delivery_OnStartDelivery(object sender, DeliveryManager.OnDeliveryEventArgs e)
    {
        _isDelivery = true;
    }
    #endregion
    
    #region Draw Cat Distance
    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(catAgent.transform.position, catStatsSO.stats.followDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(catAgent.transform.position, catStatsSO.stats.stopDistance);
    }
    #endregion
}
