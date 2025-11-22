using UnityEngine;
using UnityEngine.AI;

public class AttackState : EnemyBaseState
{
    #region Parameter
    private NavMeshAgent _navMeshAgent;
    private EnemyStatsSO _statsSO;
    private PlayerController _player;
    private FieldOfView _fov;
    private float _attackTimer;
    private float _attackDamge;
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
        base.OnEnter();
        animator.CrossFade(AttackHash, crossFadeDuration);
        
        InitializeAgent();
        Debug.Log("Enemy is attacking");
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        HandleAttack();
    }

    public override void OnExit()
    {
        _navMeshAgent.ResetPath();
    }
    #endregion

    #region Initialize
    private void InitializeAgent()
    {
        _attackTimer = _statsSO.stats.timeBetweenAttacks;
        _attackDamge = _statsSO.stats.attackDamage;
    }
    #endregion

    #region Attack State
    public void HandleAttack()
    {
        _attackTimer += Time.deltaTime;
        if(_attackTimer >= _statsSO.stats.timeBetweenAttacks)
        {
            _attackTimer = 0f;
            PerformAttack(_attackDamge);
        }
    }

    private void PerformAttack(float damage)
    {
        if(_player != null)
        {
            _player.TakeDamage(damage);
        }
    }
    #endregion
}