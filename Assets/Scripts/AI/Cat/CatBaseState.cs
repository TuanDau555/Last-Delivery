using UnityEngine;

public class CatBaseState : IState
{
    private int currentAnimHash;
    protected readonly Animator animator; 
    
    protected readonly CatAgent cat;

    protected static readonly int IdleLyingHash = Animator.StringToHash("Idle_lying_01");
    protected static readonly int IdleStandHash = Animator.StringToHash("Idle_stand");
    public static readonly int PetHash = Animator.StringToHash("Idle_lying_pet_01");
    protected static readonly int WalkHash = Animator.StringToHash("Walk");

    protected readonly float crossFadeDuration = 0.1f;

    public CatBaseState(CatAgent cat, Animator animator)
    {
        this.cat = cat;
        this.animator = animator;
    }

    protected void SetAnimation(int newHash)
    {
        if(currentAnimHash == newHash) return; // Prevent spaming call CrossFade

        currentAnimHash = newHash;
        animator.CrossFade(newHash, crossFadeDuration);
    }
    
    public virtual void FixedUpdate() { }

    public virtual void OnEnter(){}

    public virtual void OnExit(){}

    public virtual void Update(){}
}