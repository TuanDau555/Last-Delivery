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
    public AttackState(Enemy enemy, NavMeshAgent agent, PlayerController player, EnemyStatsSO statsSO) : base(enemy)
    {
        this._navMeshAgent = agent;
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
    // Logic giúp Enemy xoay để nhìn về phía Player
    private void LookAtPlayer()
    {
        if (_player == null) return;

        // Tính toán hướng từ Enemy đến Player
        Vector3 directionToPlayer = (_player.transform.position - enemy.transform.position).normalized;
        // Bỏ qua trục Y để Enemy không bị nghiêng
        directionToPlayer.y = 0;

        if (directionToPlayer != Vector3.zero)
        {
            // Tính toán Quaternion cần thiết để Enemy xoay về hướng Player
            Quaternion targetRotation = Quaternion.LookRotation(directionToPlayer);

            // Xoay từ từ về hướng Player bằng cách sử dụng Slerp
            // NOTE: Thay giá trị 5f bằng _statsSO.stats.rotationSpeed nếu có
            float rotationSpeed = 5f; 

            enemy.transform.rotation = Quaternion.Slerp(
                enemy.transform.rotation, 
                targetRotation, 
                Time.deltaTime * rotationSpeed
            );
        }
    }
    #endregion
}