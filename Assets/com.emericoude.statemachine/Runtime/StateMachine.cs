using System;
using System.Collections.Generic;

namespace Emericoude.StateMachines
{
    public class StateMachine : IState
    {
        public delegate void OnStateChangedDelegate(IState from, IState to);
        public event OnStateChangedDelegate OnStateChanged;
        
        private StateNode current;
        private readonly Dictionary<Type, StateNode> nodes = new();
        private readonly HashSet<ITransition> globalTransitions = new();

        public void OnEnter()
        {
            this.current.State?.OnEnter();
        }

        public void Update()
        {
            if (this.TryGetTransition(out var transition))
            {
                this.ChangeState(transition.To);
            }
            
            this.current.State?.Update();
        }

        public void FixedUpdate()
        {
            this.current.State?.FixedUpdate();
        }

        public void OnExit()
        {
            this.current.State?.OnExit();
        }

        public void SetState(IState state)
        {
            this.current = this.nodes[state.GetType()];
            this.current.State?.OnEnter();
        }

        public void ChangeState(IState state)
        {
            if (state == this.current.State) return;

            var previousState = this.current.State;
            var nextState = this.nodes[state.GetType()].State;
            
            previousState.OnExit();
            nextState.OnEnter();

            this.current = this.nodes[state.GetType()];
            this.OnStateChanged?.Invoke(previousState, nextState);
        }

        private bool TryGetTransition(out ITransition transition)
        {
            foreach (var globalTransition in this.globalTransitions)
            {
                if (!globalTransition.Condition.Evaluate()) continue;
                transition = globalTransition;
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

        public void AddTransition(IState from, IState to, IPredicate condition)
        {
            this.GetOrAddNode(from).AddTransition(this.GetOrAddNode(to).State, condition);
        }

        public void AddGlobalTransition(IState to, IPredicate condition)
        {
            this.globalTransitions.Add(new Transition(this.GetOrAddNode(to).State, condition));
        }

        private StateNode GetOrAddNode(IState state)
        {
            if (!this.nodes.TryGetValue(state.GetType(), out var node))
            {
                node = new StateNode(state);
                this.nodes.Add(state.GetType(), node);
            }

            return node;
        }
        
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