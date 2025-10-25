using System;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyBaseState
{
    #region Parameter
    private NavMeshAgent _navMeshAgent;
    private EnemyStatsSO _statsSO;
    private PlayerController _player;
    private FieldOfView _fov;
    private float _viewRadius;
    private float _viewAngle;
    #endregion

    #region Constructor 
    public ChaseState(Enemy enemy, NavMeshAgent agent, PlayerController player, FieldOfView fov, EnemyStatsSO statsSO) : base(enemy)
    {
        this._navMeshAgent = agent;
        this._player = player;
        this._fov = fov;
        this._statsSO = statsSO;
    }
    #endregion

    #region Execute
    public override void OnEnter()
    {
        Debug.Log($"{enemy.name} is chasing");
        InitializeAgent();

        _navMeshAgent.SetDestination(_player.transform.position);
    }

    public override void Update()
    {
        ChasePlayer();
    }


    public override void OnExit()
    {
        _navMeshAgent.ResetPath();
        _navMeshAgent.isStopped = true; // Dừng agent khi thoát
    }
    #endregion

    #region Agent
    private void InitializeAgent()
    {
        if (_navMeshAgent == null || _player == null) return;

        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = _statsSO.stats.chaseSpeed;
        _navMeshAgent.stoppingDistance = _statsSO.stats.attackDistance;
    }
    #endregion

    #region States
    private void ChasePlayer()
    {
        // Đảm bảo Agent di chuyển
        if (_navMeshAgent.isStopped)
        {
            _navMeshAgent.isStopped = false;
        }

        // Estimate player's move and time, which enemy needs to go to player position  
        float timeToPlayer = Vector3.Distance(_player.transform.position, _navMeshAgent.transform.position)
        / _statsSO.stats.chaseSpeed;

        // To avoid Prediction player move to far
        // if player goes further, the enemy can only prediction the amount of time (here is movePredictionTime)  
        if (timeToPlayer > _statsSO.stats.movePredictionTime)
        {
            timeToPlayer = _statsSO.stats.movePredictionTime;
        }

        // Prediction the position that player may come
        // This current position plus with Distance that player may go in that time
        /// Summary:
        ///      If the player runs in direction X with an average speed of 5m/s and it takes me 1s to catch up to the same place → then the player has gone another 5m, I should run there.
        Vector3 targetPosition = _player.transform.position
                        + _player.GetAverageVelocity() * timeToPlayer;


        Vector3 directionToPlayer = (_player.transform.position - _navMeshAgent.transform.position).normalized;
        Vector3 directionToTarget = (targetPosition - _navMeshAgent.transform.position).normalized;

        float dot = Vector3.Dot(directionToPlayer, directionToTarget); // cosin angle between 2 vector

        // if enemy's prediction direction is too far from current player's direction...
        // To Prevent run infront of player
        if (dot < _statsSO.stats.movePredictionThreshold)
        {
            // ...No need to predict go to player position  
            targetPosition = _player.transform.position;
        }

        _navMeshAgent.SetDestination(targetPosition);

    }
    #endregion
        
    // #region Change State - Đã loại bỏ
    // private void InAttackRange() { ... }
    // #endregion
}