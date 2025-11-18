using UnityEngine;
using UnityEngine.AI;

public class CatIdleState : CatBaseState
{
    private NavMeshAgent _catAgent;
    public CatIdleState(CatAgent cat, Animator animator, NavMeshAgent agent) : base(cat, animator)
    {
        this._catAgent = agent;
    }

    #region Execute
    public override void OnEnter()
    {
        SetAnimation(IdleLyingHash);
        Debug.Log("Cat is Idling");
    }

    public override void OnExit()
    {
        _catAgent.ResetPath();
    }
    #endregion
}