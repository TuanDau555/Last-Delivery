using UnityEngine;
using UnityEngine.AI;

public class CatIdleState : CatBaseState
{
    private NavMeshAgent _catAgent;
    private Vector3 _idlePos;
    public CatIdleState(CatAgent cat, Animator animator, NavMeshAgent agent) : base(cat, animator)
    {
        this._catAgent = agent;
        _idlePos = cat.catIdlePos;
    }

    #region Execute
    public override void OnEnter()
    {
        animator.CrossFade(IdleLyingHash, 0.2f);

        _catAgent.Warp(_idlePos);
        
        _catAgent.isStopped = true;
        

        Debug.Log("Cat is Idling");
    }

    public override void OnExit()
    {
        _catAgent.ResetPath();
    }
    #endregion
}