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
        var chaseState = new ChaseState(enemy, agent, player, fov, enemyStatsSO);
        // CẬP NHẬT: Thêm player và enemyStatsSO vào AttackState
        var attackState = new AttackState(enemy, agent, player, enemyStatsSO);

        // Transition: Any -> Patrol (Nếu không nhìn thấy player)
        Any(patrolState, new FuncPredicate(() => !fov.canSeePlayer));
        
        // Transition: Patrol -> Chase (Nếu nhìn thấy player)
        At(patrolState, chaseState, new FuncPredicate(() => fov.canSeePlayer));
        
        // Transition: Chase -> Attack (Nếu ở trong tầm đánh)
        At(chaseState, attackState, new FuncPredicate(() => fov.inAttackRange));

        // Transition: Attack -> Chase (Nếu nhìn thấy player nhưng ra khỏi tầm đánh)
        At(attackState, chaseState, new FuncPredicate(() => fov.canSeePlayer && !fov.inAttackRange));
        
        // Set Initial State
        stateMachine.SetState(patrolState);
    }
    #endregion
}