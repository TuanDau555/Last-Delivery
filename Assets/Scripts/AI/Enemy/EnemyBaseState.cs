
using UnityEngine;

public abstract class EnemyBaseState : IState
{

    protected readonly Enemy enemy;
    protected readonly Animator animator;

    protected static readonly int PatrolHash = Animator.StringToHash("Walk");
    protected static readonly int IdleHash = Animator.StringToHash("Idle");
    protected static readonly int ChaseHash = Animator.StringToHash("Chase");
    protected static readonly int AttackHash = Animator.StringToHash("Attacking1");

    protected readonly float crossFadeDuration = 0.1f;

    public EnemyBaseState(Enemy enemy, Animator animator)
    {
        this.enemy = enemy;
        this.animator = animator;
    }
    
    public virtual void FixedUpdate() { }

    public virtual void OnEnter(){}

    public virtual void OnExit(){}

    public virtual void Update(){}
}