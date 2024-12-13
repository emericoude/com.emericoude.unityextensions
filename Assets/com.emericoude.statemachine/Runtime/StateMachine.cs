using System;
using System.Collections.Generic;

namespace Emericoude.StateMachines
{
    /// <summary> A lightweight state machine that is also itself a state that can be used in a hierarchical manner.
    /// At the current time, this tool is built mainly to be managed in code. The manager of the state machine should
    /// create the state machine and its states and its transitions. Although it is possible to make states which
    /// are MonoBehaviours for example. </summary>
    public class StateMachine : IState
    {
        public delegate void OnStateChangedDelegate(IState from, IState to);
        
        /// <summary> Invoked when a state change occurs. </summary>
        public event OnStateChangedDelegate OnStateChanged;
        
        private StateNode current;
        private readonly Dictionary<Type, StateNode> nodes = new();
        private readonly HashSet<ITransition> fromAnyTransitions = new();

        public StateMachine() { }
        
        public StateMachine(IState defaultState)
        {
            this.current = this.GetOrAddNode(defaultState);
        }

        public virtual void OnInit()
        {
            foreach (var node in this.nodes.Values)
            {
                node.State.OnInit();
            }
        }

        public virtual void OnEnter()
        {
            this.current.State?.OnEnter();
        }

        public virtual void Update()
        {
            if (this.TryEvaluateTransitions(out var transition))
            {
                this.ChangeState(transition.To);
            }
            
            this.current.State?.Update();
        }

        public virtual void FixedUpdate()
        {
            this.current.State?.FixedUpdate();
        }

        public virtual void OnExit()
        {
            this.current.State?.OnExit();
        }
        
        /// <summary> Hard sets the current state. This does NOT exit the current state if there is one. </summary>
        public void SetState<T>() where T : IState
        {
            this.current = this.nodes[typeof(T)];
            this.current.State?.OnEnter();
        }

        /// <summary> Hard sets the current state. This does NOT exit the current state if there is one. </summary>
        public void SetState(IState state)
        {
            this.current = this.nodes[state.GetType()];
            this.current.State?.OnEnter();
        }

        /// <summary> Transitions from the current state to a new one. This invokes OnExit for the previous,
        /// OnEnter for the new, and the OnStateChanged event. </summary>
        public void ChangeState(IState to)
        {
            if (to == this.current.State) return;

            var previousState = this.current.State;
            var nextState = this.nodes[to.GetType()].State;
            
            previousState.OnExit();
            nextState.OnEnter();

            this.current = this.nodes[to.GetType()];
            this.OnStateChanged?.Invoke(previousState, nextState);
        }

        /// <summary> Add a transition from one state to another with a condition. This also registers states
        /// to the state machine if they aren't already. </summary>
        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            this.GetOrAddNode(from).AddTransition(this.GetOrAddNode(to).State, condition);
        }

        /// <summary> Adds a "from any state" transition to a state with a condition. </summary>
        public void AddFromAnyTransition(IState to, IPredicate condition)
        {
            this.fromAnyTransitions.Add(new Transition(this.GetOrAddNode(to).State, condition));
        }
        
        /// <summary> Checks global and the current state's transitions to see if we should transition to a new state. </summary>
        /// <param name="transition"> If we return true, the transition that we should do; otherwise null. </param>
        /// <remarks> "From Any" transitions are evaluated before the current state's transitions. </remarks>
        /// <returns> True if a transition should be done; otherwise false. </returns>
        private bool TryEvaluateTransitions(out ITransition transition)
        {
            foreach (var fromAnyTransition in this.fromAnyTransitions)
            {
                if (!fromAnyTransition.Condition.Evaluate()) continue;
                transition = fromAnyTransition;
                return true;
            }

            foreach (var currentStateTransitions in this.current.Transitions)
            {
                if (!currentStateTransitions.Condition.Evaluate()) continue;
                transition = currentStateTransitions;
                return true;
            }

            transition = null;
            return false;
        }

        /// <summary> Gets the node for this state. If none exist, create one. </summary>
        /// <returns> The StateNode for this state. </returns>
        private StateNode GetOrAddNode(IState state)
        {
            if (!this.nodes.TryGetValue(state.GetType(), out var node))
            {
                node = new StateNode(state);
                this.nodes.Add(state.GetType(), node);
            }

            return node;
        }
        
        /// <summary> A wrapper containing a state and its transitions. </summary>
        private class StateNode
        {
            public IState State { get; }
            public HashSet<ITransition> Transitions { get; }

            public StateNode(IState state)
            {
                this.State = state;
                this.Transitions = new HashSet<ITransition>();
            }

            public void AddTransition(IState to, IPredicate condition)
            {
                this.Transitions.Add(new Transition(to, condition));
            }
        }
    }
}