using System;
using System.Collections.Generic;

public class StateMachine
{
    #region Parameters
    private StateNode _currentNode;
    private Dictionary<Type, StateNode> _nodes = new();
    private HashSet<ITransition> _anyTransition = new();
    #endregion

    #region Execute
    public void Update()
    {
        var transition = GetTransition();
        if (transition != null)
        {
            ChangeState(transition.To);
        }
        _currentNode.State?.Update();
    }

    public void FixedUpdate()
    {
        _currentNode.State?.FixedUpdate();
    }
    #endregion

    #region SetState
    public void SetState(IState state)
    {
        _currentNode = _nodes[state.GetType()];
        _currentNode.State?.OnEnter(); // Set if not null
    }

    private void ChangeState(IState state)
    {
        if (state == _currentNode.State) return; // Prevent jitter 

        var previousState = _currentNode.State;
        var nextState = _nodes[state.GetType()].State;

        // if previous State not null
        previousState?.OnExit();

        // if not null State not null
        nextState?.OnEnter();
    }
    #endregion

    #region Transition
    private ITransition GetTransition()
    {
        // Get Any Transition
        foreach (var transition in _anyTransition)
            if (transition.Condition.Evaluate())
                return transition;

        // Transition in this node
        foreach (var transition in _currentNode.Transitions)
            if (transition.Condition.Evaluate())
                return transition;

        return null;
    }

    public void AddTransition(IState from, IState to, IPredicate condition)
    {
        GetOrAddNode(from).AddTransition(GetOrAddNode(to).State, condition);
    }

    public void AddAnyTransition(IState to, IPredicate condition)
    {
        _anyTransition.Add(new Transition(GetOrAddNode(to).State, condition));
    }
    #endregion

    #region State Node
    private StateNode GetOrAddNode(IState state)
    {
        var node = _nodes.GetValueOrDefault(state.GetType());
        if (node == null)
        {
            node = new StateNode(state);
            _nodes.Add(state.GetType(), node);
        }

        return node;
    }   
    
    /// <summary>
    /// A StateNode is encloses a State
    /// And each StateNode contain many difference Transition to difference State  
    /// </summary>
    private class StateNode
    {
        public IState State { get; }
        public HashSet<ITransition> Transitions { get; }
        public StateNode(IState state)
        {
            this.State = state;
            Transitions = new HashSet<ITransition>();
        }
        public void AddTransition(IState to, IPredicate condition)
        {
            Transitions.Add(new Transition(to, condition));
        }
    }
    #endregion
}