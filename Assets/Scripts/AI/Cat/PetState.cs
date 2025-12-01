using UnityEngine;
using UnityEngine.AI;

public class PetState : CatBaseState
{
    private NavMeshAgent _catAgent;
    
    public PetState(CatAgent cat, Animator animator, NavMeshAgent agent) : base(cat, animator)
    {
        _catAgent = agent;
    }

    #region Execute
    public override void OnEnter()
    {
        base.OnEnter();
        Debug.Log("Cat being petting");
        animator.CrossFade(PetHash, crossFadeDuration);
    }

    public override void OnExit()
    {
        base.OnExit();
        _catAgent.ResetPath();
    }
    #endregion
}