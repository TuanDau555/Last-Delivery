
public abstract class EnemyBaseState : IState
{

    protected readonly Enemy enemy;
    // TODO: make a read only field Animator

    // TODO: Animation to Hash

    protected readonly float crossFadeDuration = 0.1f;

    public EnemyBaseState(Enemy enemy)
    {
        this.enemy = enemy;
    }
    
    public virtual void FixedUpdate() { }

    public virtual void OnEnter(){}

    public virtual void OnExit(){}

    public virtual void Update(){}
}