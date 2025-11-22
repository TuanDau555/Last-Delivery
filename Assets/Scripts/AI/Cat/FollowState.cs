using UnityEngine;
using UnityEngine.AI;

public class FollowState : CatBaseState
{
    #region Parameter
    private NavMeshAgent _catAgent;
    private Transform _playerTransform;
    private CatStatsSO _catStatsSO;

    #endregion
    
    public FollowState(CatAgent cat, Animator animator, NavMeshAgent agent, Transform playerTransform, CatStatsSO statsSO) : base(cat, animator)
    {
        this._catAgent = agent;
        this._playerTransform = playerTransform;
        this._catStatsSO = statsSO;
    }
    #region Execute
    public override void OnEnter()
    {
        Debug.Log("Cat is Follow player");
        InitializeAgent();
        animator.CrossFade(IdleStandHash, 0.2f);

        FollowPlayer();
    }

    public override void OnExit()
    {
        _catAgent.ResetPath();
    }

    public override void FixedUpdate()
    {
        FollowPlayer();
    }
    #endregion

    private void InitializeAgent()
    {
        if (_catAgent == null) return;

        _catAgent.stoppingDistance = _catStatsSO.stats.stopDistance;
    }
    
    #region Cat Follow
    // TODO: ONLY follow player when in specific STATE
    private void FollowPlayer()
    {
        Vector3 catPos = _catAgent.transform.position;
        // Get player Distance between player and cat
        float playerDistance = Vector3.Distance(catPos, _playerTransform.position);

        // If the player moves out of the cat's following range
        if (playerDistance > _catStatsSO.stats.followDistance)
        {
            // Cat follow
            SetAnimation(WalkHash);
            _catAgent.SetDestination(_playerTransform.position);

        }
        else if (playerDistance <= _catAgent.stoppingDistance)
        {
            // cat just standing there
            SetAnimation(IdleStandHash);
            _catAgent.ResetPath();
        }
    }
    #endregion
}