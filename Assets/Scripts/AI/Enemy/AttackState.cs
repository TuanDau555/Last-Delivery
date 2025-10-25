using UnityEngine;
using UnityEngine.AI;

public class AttackState : EnemyBaseState
{
    #region Parameter
    private NavMeshAgent _navMeshAgent;
    private EnemyStatsSO _statsSO;
    private PlayerController _player;
    private FieldOfView _fov;
    #endregion

    #region Constructor
    public AttackState(Enemy enemy, NavMeshAgent agent) : base(enemy)
    {
        // TODO: Add EnemyStatsSO, PlayerController, FOV to constructor
        this._navMeshAgent = agent;
    }
    #endregion

    #region Execute
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Enemy is attacking");
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void OnExit()
    {
        _navMeshAgent.ResetPath();
    }
    #endregion

    #region Initialize
    // TODO: Initialize Enemy stats from SO and call it from OnEnter()
    #endregion

    #region Attack State
    // TODO: Enemy Look at player when in attack State (Look at camera)

    // TODO: Enemy attack player and player decreasing HP
    #endregion
}