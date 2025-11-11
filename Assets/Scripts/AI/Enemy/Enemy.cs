using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStatsSO enemyStatsSO;
    [SerializeField] private NavMeshAgent enemyAgent;
    private const string TAG = "Player";
    private StateMachine _stateMachine;

    public StateMachine StateMachine => _stateMachine;
    public EnemyStatsSO StatsSO => enemyStatsSO;
    #region Execute
    void Start()
    {
        _stateMachine = new StateMachine();
        EnemyState(this, enemyAgent, _stateMachine);
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

    #region Add Transition
    void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);
    #endregion

    #region Enemy State
    /// <summary>
    /// We add State here
    /// </summary>
    /// <param name="enemy">This Enemy</param>
    /// <param name="agent">this agent</param>
    /// <param name="stateMachine">this state machine</param> <summary>
    void EnemyState(Enemy enemy, NavMeshAgent agent, StateMachine stateMachine)
    {
        FieldOfView fov = GetComponent<FieldOfView>();
        PlayerController player = GameObject.FindGameObjectWithTag(TAG).GetComponent<PlayerController>();

        var patrolState = new PatrolState(enemy, agent, enemyStatsSO);
        var chaseState = new ChaseState(enemy, agent, player, fov, enemyStatsSO);
        var attackState = new AttackState(enemy, agent, enemyStatsSO, player);

        Any(patrolState, new FuncPredicate(() => !fov.canSeePlayer));
        At(patrolState, chaseState, new FuncPredicate(() => fov.canSeePlayer));
        //At(chaseState, patrolState, new FuncPredicate(() => !fov.canSeePlayer));
        // TODO: at chase state to attack state if in attack range
        Any(attackState, new FuncPredicate(() => fov.inAttackRange));
        At(attackState, chaseState, new FuncPredicate(() => fov.canSeePlayer && !fov.inAttackRange));
        // Set Initial State
        stateMachine.SetState(patrolState);
    }
    #endregion
}
