using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStatsSO enemyStatsSO;
    [SerializeField] private NavMeshAgent agent;

    private StateMachine _stateMachine;

    #region Execute
    void Start()
    {
        _stateMachine = new StateMachine();

    }

    void Update()
    {
        _stateMachine.Update();
    }

    void FixedUpdate()
    {
        _stateMachine.FixedUpdate();
    }
    #endregion

    #region Add Transition
    void At(IState from, IState to, IPredicate condition) => _stateMachine.AddTransition(from, to, condition);
    void Any(IState to, IPredicate condition) => _stateMachine.AddAnyTransition(to, condition);
    #endregion

    #region Enemy State
    /// <summary>
    /// We add Sate here
    /// </summary>
    /// <param name="enemy">This Enemy</param>
    /// <param name="agent">this agent</param>
    /// <param name="stateMachine">this state machine</param> <summary>
    void EnemyState(Enemy enemy, NavMeshAgent agent, StateMachine stateMachine)
    {

    }
    #endregion
}
