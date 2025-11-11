using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

[RequireComponent(typeof(NavMeshAgent))]
public class CatAgent : BaseInteract, ISaveable
{
    #region KEYS
    private const string CAT_MOOD = "CatMood";
    #endregion
    
    #region Parameter
    [Header("Pref")]
    [SerializeField] private CatStatsSO catStatsSO;
    [SerializeField] private NavMeshAgent catAgent;
    [SerializeField] private Transform playerTransform;

    [Space(10)]
    [Header("UI / Display")]
    [SerializeField] private SpriteRenderer orderDisplay; // Sprite on cat back
    [SerializeField] private Slider catMoodBar;

    [Space(10)]
    [SerializeField] private Vector3 catIdlePos;

    private StateMachine _stateMachine;
    private bool _isDelivery;
    private float currentMoodBar;
    #endregion

    #region Execute
    void Start()
    {
        _stateMachine = new StateMachine();
        CatState(this, catAgent, _stateMachine);
        DeliveryManager.Instance.OnStartDelivery += Delivery_OnStartDelivery;
        DeliveryManager.Instance.OnStopDelivery += Delivery_OnStopDelivery;

        InitializeCatStats();
    }

    void OnDisable()
    {
        DeliveryManager.Instance.OnStartDelivery -= Delivery_OnStartDelivery;
        DeliveryManager.Instance.OnStopDelivery -= Delivery_OnStopDelivery;
    }

    void Update()
    {
        _stateMachine.Update();
        UpdateMood();
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

            UIManager.Instance.ShowDeliveryTableFeedback(table.name);
            
            // Assign the sprite to cat
            if (orderDisplay != null && cargoObjectSO.cargoOrderSprite != null && DeliveryManager.Instance.GetWaitingList().Count > 0)
            {
                orderDisplay.sprite = cargoObjectSO.cargoOrderSprite;
                orderDisplay.enabled = true;

                DeliveryManager.Instance.TriggerStartDelivery(cargoObjectSO, table);
            }
            else
            {
                UIManager.Instance.ShowAlreadyDeliverObject();
            }
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

        Any(idleState, new FuncPredicate(() => _isDelivery == false));
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

    private void Delivery_OnStopDelivery(object sender, EventArgs e)
    {
        _isDelivery = false;
        transform.position = catIdlePos; // TODO: set the cat position at it bed 
    }
    #endregion

    #region Init and Update Stats
    void InitializeCatStats()
    {
        catMoodBar.maxValue = catStatsSO.stats.catMoodBar;
        currentMoodBar = catMoodBar.maxValue;
        catMoodBar.value = currentMoodBar;
    }

    void UpdateMood()
    {
        if (catMoodBar.value >= 0)
            catMoodBar.value -= Time.deltaTime * 2;
    }

    public void ApplyBuff(float buffAmount)
    {
        currentMoodBar = Mathf.Clamp(currentMoodBar + buffAmount, 0, catMoodBar.maxValue);
        catMoodBar.value = currentMoodBar;
    }

    public void UpgradeMaxMood(float upgradeValue)
    {
        catMoodBar.maxValue += upgradeValue;
        currentMoodBar = catMoodBar.maxValue;
        catMoodBar.value = currentMoodBar;
    }
    #endregion

    #region Save and Load
    public void Save(SaveData data)
    {
        data.Set(CAT_MOOD, catMoodBar.value);
    }
    
    public void Load(SaveData data)
    {
        catMoodBar.value = data.Get<float>(CAT_MOOD, catMoodBar.value);
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
