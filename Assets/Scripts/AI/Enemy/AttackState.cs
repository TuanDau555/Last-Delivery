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
    public AttackState(Enemy enemy, NavMeshAgent agent, FieldOfView fov, PlayerController player, EnemyStatsSO statsSO) : base(enemy)
    {
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
        // Dừng di chuyển ngay lập tức khi vào trạng thái Attack
        _navMeshAgent.isStopped = true;
        Debug.Log("Enemy is attacking");
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    public override void Update()
    {
        base.Update();
        // Enemy Look at player when in attack State
        LookAtPlayer(); 

        // TODO: Enemy attack player and player decreasing HP (Logic tấn công sẽ được thêm vào đây)
    }

    public override void OnExit()
    {
        _navMeshAgent.isStopped = false;
        _navMeshAgent.ResetPath();
    }
    #endregion

    #region Attack State
    // TODO: Enemy attack player and player decreasing  anh 
    #endregion
}