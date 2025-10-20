public class CatBaseState : IState
{

    protected readonly CatAgent cat;
    // TODO: make a read only field Animator

    // TODO: Animation to Hash

    protected readonly float crossFadeDuration = 0.1f;

    public CatBaseState(CatAgent cat)
    {
        this.cat = cat;
    }

    public virtual void FixedUpdate() { }

    public virtual void OnEnter(){}

    public virtual void OnExit(){}

    public virtual void Update(){}
}