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
    public AttackState(Enemy enemy, Animator animator, NavMeshAgent agent, FieldOfView fov, PlayerController player, EnemyStatsSO statsSO) : base(enemy, animator)
    {
        // TODO: Add EnemyStatsSO, PlayerController, FOV to constructor
        this._navMeshAgent = agent;
        this._fov = fov;
        this._player = player;
        this._statsSO = statsSO;
    }
    #endregion

    #region Execute
    public override void OnEnter()
    {
        animator.CrossFade(AttackHash, crossFadeDuration);
        
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
    // TODO: Enemy attack player and player decreasing  anh 
    #endregion
}