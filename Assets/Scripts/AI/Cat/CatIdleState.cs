using UnityEngine;
using UnityEngine.AI;

public class CatIdleState : CatBaseState
{
    private NavMeshAgent _catAgent;
    public CatIdleState(CatAgent cat, NavMeshAgent agent) : base(cat)
    {
        this._catAgent = agent;
    }

    #region Execute
    public override void OnEnter()
    {
        Debug.Log("Cat is Idling");
    }

    public override void OnExit()
    {
        _catAgent.ResetPath();
    }
    #endregion
}