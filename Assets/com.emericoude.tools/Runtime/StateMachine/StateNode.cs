using System.Collections.Generic;

namespace Emericoude.StateMachine
{
    /// <summary> A wrapper containing a state and its transitions. </summary>
    public class StateNode
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