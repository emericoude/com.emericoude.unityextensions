using System;
using System.Collections.Generic;

namespace Emericoude.StateMachine
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
        
        private readonly Dictionary<Type, StateNode> nodes = new();
        private readonly HashSet<ITransition> fromAnyTransitions = new();
        private StateNode current;

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

        public virtual void OnEnter(EventArgs args = null)
        {
            this.current.State?.OnEnter(args);
        }

        public virtual void OnUpdate()
        {
            if (this.TryEvaluateTransitions(out var transition))
            {
                this.ChangeState(transition.To, transition.FuncArgs?.Invoke());
            }
            
            this.current.State?.OnUpdate();
        }

        public virtual void OnFixedUpdate()
        {
            this.current.State?.OnFixedUpdate();
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
        public void ChangeState(IState to, EventArgs args = null)
        {
            var previousState = this.current.State;
            var nextState = this.nodes[to.GetType()].State;
            
            previousState.OnExit();
            nextState.OnEnter(args);

            this.current = this.nodes[to.GetType()];
            this.OnStateChanged?.Invoke(previousState, nextState);
        }

        /// <summary> Adds a state node to the state machine. You don't need to use this, you can use <see cref="AddTransition"/> directly. </summary>
        /// <returns> The added state. </returns>
        public IState AddState(IState state)
        {
            return this.GetOrAddNode(state).State;
        }

        /// <summary> Add a transition from one state to another with a condition. This also registers states
        /// to the state machine if they aren't already. </summary>
        /// <param name="from"> The state to come from. </param>
        /// <param name="to"> The state to go to, if the condition is true. </param>
        /// <param name="condition"> The condition to evaluate for transition. See <see cref="FuncPredicate"/> or <see cref="SingleFrameTriggerPredicate"/>. </param>
        /// <param name="args"> Arguments for you to pass to the state when entering it. Implement a custom <see cref="EventArgs"/> to pass in information when entering. You can then do "if (args is MyArgs myArgs)" in OnEnter. </param>
        public void AddTransition(IState from, IState to, IPredicate condition, Func<EventArgs> args = null)
        {
            this.GetOrAddNode(from).AddTransition(this.GetOrAddNode(to).State, condition, args);
        }

        /// <summary> Adds a "from any state" transition to a state with a condition. </summary>
        /// <param name="to"> The state to go to, if the condition is true. </param>
        /// <param name="canTransitionToSelf"> If true, the to state can transition to itself using this transition. </param>
        /// <param name="condition"> The condition to evaluate for transition. See <see cref="FuncPredicate"/> or <see cref="SingleFrameTriggerPredicate"/>. </param>
        /// <param name="args"> Arguments for you to pass to the state when entering it. Implement a custom <see cref="EventArgs"/> to pass in information when entering. You can then do "if (args is MyArgs myArgs)" in OnEnter. </param>
        public void AddFromAnyTransition(IState to, IPredicate condition, bool canTransitionToSelf = false, Func<EventArgs> args = null)
        {
            this.fromAnyTransitions.Add(new Transition(
                this.GetOrAddNode(to).State, 
                condition, 
                canTransitionToSelf,
                args
            ));
        }
        
        /// <summary> Checks global and the current state's transitions to see if we should transition to a new state. </summary>
        /// <param name="transition"> If we return true, the transition that we should do; otherwise null. </param>
        /// <remarks> "From Any" transitions are evaluated before the current state's transitions. </remarks>
        /// <returns> True if a transition should be done; otherwise false. </returns>
        private bool TryEvaluateTransitions(out ITransition transition)
        {
            foreach (var fromAnyTransition in this.fromAnyTransitions) {
                if (!fromAnyTransition.CanTransitionToSelf && this.current.State == fromAnyTransition.To) continue;
                if (!fromAnyTransition.Condition.Evaluate()) continue;
                transition = fromAnyTransition;
                return true;
            }

            foreach (var currentStateTransitions in this.current.Transitions) {
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

        public bool TryGetState<T>(out T state) where T : IState 
        {
            if (this.nodes.TryGetValue(typeof(T), out var node))
            {
                state = (T)node.State;
                return true;
            }

            state = default(T);
            return false;
        }
        
        public HashSet<ITransition> GetFromAnyTransitions() => this.fromAnyTransitions;
        public Dictionary<Type, StateNode> GetStateNodes() => this.nodes;
        public StateNode GetCurrentStateNode() => this.current;
        public IState GetCurrentState() => this.current?.State;
    }
}