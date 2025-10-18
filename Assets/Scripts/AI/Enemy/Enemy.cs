using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStatsSO enemyStatsSO;
    [SerializeField] private NavMeshAgent enemyAgent;

    private const string TAG = "Player";
    private StateMachine _stateMachine;

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
        var chaseState = new ChaseState(enemy, agent, player.transform, fov, enemyStatsSO);

        // Any(patrolState, new FuncPredicate(() => !fov.canSeePlayer));
        At(patrolState, chaseState, new FuncPredicate(() => fov.canSeePlayer));
        At(chaseState, patrolState, new FuncPredicate(() => !fov.canSeePlayer));
        
        // Set Initial State
        stateMachine.SetState(patrolState);
    }
    #endregion
}
