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
    #endregion

    #region Constructor
    public AttackState(Enemy enemy, NavMeshAgent agent, FieldOfView fov, PlayerController player, EnemyStatsSO statsSO) : base(enemy)
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
        Debug.Log("Enemy is attacking");
        _navMeshAgent.isStopped = true;
        _attackTimer = _statsSO.stats.timeBetweenAttacks;
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();

        LookAtPlayer();


        // Xử lý logic tấn công
        HandleAttack();
    }

    public override void OnExit()
    {
        _navMeshAgent.ResetPath();
    }
    #endregion
    #region Attack State
    private void LookAtPlayer()
    {
        if (_player == null) return;
        Vector3 direction = (_player.transform.position - enemy.transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, lookRotation, Time.deltaTime * 5f);
    }

    private void HandleAttack()
    {
        _attackTimer += Time.deltaTime;
        if (_attackTimer >= _statsSO.stats.timeBetweenAttacks)
        {
            _attackTimer = 0f;
            PerformAttack();
        }
    }

    private void PerformAttack()
    {
        // GÂY SÁT THƯƠNG CHO NGƯỜI CHƠI
        if (_player != null)
        {
            Debug.Log("Enemy hits player for " + _statsSO.stats.attackDamage + " damage!");
            _player.TakeDamage(_statsSO.stats.attackDamage);
        }
    }
    #endregion
}

    #region Initialize
    // TODO: Initialize Enemy stats from SO and call it from OnEnter()
    #endregion

    #region Attack State
    // TODO: Enemy attack player and player decreasing  anh 
    #endregion
