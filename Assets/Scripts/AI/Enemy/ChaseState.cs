using System;
using UnityEngine;
using UnityEngine.AI;

public class ChaseState : EnemyBaseState
{
    #region Parameter
    private NavMeshAgent _navMeshAgent;
    private Transform _player;
    private FieldOfView _fov;
    private float _viewRadius;
    private float _viewAngle;
    private float _attackDistance;
    private float _enemySpeed;
    #endregion

    #region Constructor 
    public ChaseState(Enemy enemy, NavMeshAgent agent, Transform player, FieldOfView fov, float attackDistance, float chaseSpeed) : base(enemy)
    {
        this._navMeshAgent = agent;
        this._player = player;
        this._fov = fov;
        this._attackDistance = attackDistance;
        this._enemySpeed = chaseSpeed;
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

        _navMeshAgent.speed = _enemySpeed;
        _navMeshAgent.stoppingDistance = _attackDistance;

    }

    private void ChasePlayer()
    {
        // repetitively updated player position 
        _navMeshAgent.SetDestination(_player.position);

        float distance = Vector3.Distance(_navMeshAgent.transform.position, _player.transform.position);

        // In attack range => stop
        if (distance <= _attackDistance)
        {
            _navMeshAgent.ResetPath();
            return;
        } 
    }
}