using System;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyBaseState
{
    #region Parameter
    private NavMeshAgent _navMeshAgent;
    private EnemyStatsSO _statsSO;
    private Transform _player;
    private FieldOfView _fov;
    private float _viewRadius;
    private float _viewAngle;
    #endregion

    #region Constructor 
    public ChaseState(Enemy enemy, NavMeshAgent agent, Transform player, FieldOfView fov, EnemyStatsSO statsSO) : base(enemy)
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

        _navMeshAgent.SetDestination(_player.position);
    }

    public override void Update()
    {
        ChasePlayer();
    }


    public override void OnExit()
    {
        _navMeshAgent.ResetPath();
    }
    #endregion

    private void InitializeAgent()
    {
        if (_navMeshAgent == null || _player == null) return;

        _navMeshAgent.isStopped = false;
        _navMeshAgent.speed = _statsSO.stats.chaseSpeed;
        _navMeshAgent.stoppingDistance = _statsSO.stats.attackDistance;

    }

    private void ChasePlayer()
    {
        // repetitively updated player position 
        _navMeshAgent.SetDestination(_player.position);

        float distance = Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position);

        // In attack range => stop
        if (distance <= _statsSO.stats.attackDistance)
        {
            _navMeshAgent.ResetPath();
            return;
        } 
    }
}