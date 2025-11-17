using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [Tooltip("Area that Enemy Patrol")]
    [SerializeField] private Transform patrolRegion;
    [SerializeField] private EnemyStatsSO enemyStatsSO;
    [SerializeField] private NavMeshAgent enemyAgent;
    [SerializeField] private Animator animator;
    private const string TAG = "Player";
    private StateMachine _stateMachine;
    PlayerController _player;
    FieldOfView _fov;



    #region Execute
    void Start()
    {
        _stateMachine = new StateMachine();

        _player = GameObject.FindGameObjectWithTag(TAG).GetComponent<PlayerController>();
        animator = GetComponent<Animator>();
        _fov = GetComponent<FieldOfView>();
        
        EnemyState(this, enemyAgent, _stateMachine, _player, _fov);
    }

    void Update()
    {
        _stateMachine.Update();
        if(_fov.canSeePlayer || _fov.inAttackRange)
            LookAtPlayer(_player.transform.position, _fov.directionToTarget, enemyStatsSO.stats.lookSpeed);

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
    void EnemyState(Enemy enemy, NavMeshAgent agent, StateMachine stateMachine, PlayerController player, FieldOfView fov)
    {

        var patrolState = new PatrolState(enemy, animator, agent, enemyStatsSO, patrolRegion);
        var chaseState = new ChaseState(enemy, animator, agent, player, fov, enemyStatsSO);
        var attackState = new AttackState(enemy, animator, agent, fov, player, enemyStatsSO);

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

    private void LookAtPlayer(Vector3 targetPos, Vector3 directionToPlayer, float rotationSpeed)
    {
        if (_player == null) return;

        // Tính toán hướng từ Enemy đến Player
        directionToPlayer = (targetPos - transform.position).normalized;
        // Bỏ qua trục Y để Enemy không bị nghiêng
        directionToPlayer.y = 0;

        if (directionToPlayer != Vector3.zero)
        {
            // Tính toán Quaternion cần thiết để Enemy xoay về hướng Player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Xoay từ từ về hướng Player bằng cách sử dụng Slerp
            // NOTE: Thay giá trị 5f bằng _statsSO.stats.rotationSpeed nếu có 

            transform.rotation = Quaternion.Slerp(
                transform.rotation, 
                targetRotation, 
                Time.deltaTime * rotationSpeed
            );
        }
    }
}
