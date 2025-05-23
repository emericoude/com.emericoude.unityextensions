using System;

namespace Emericoude.StateMachine
{
    /// <summary> Concrete implementation of a ITransition used by state machines, which should cover most needs. </summary>
    public class Transition : ITransition
    {
        public IState To { get; }
        public IPredicate Condition { get; }
        public bool CanTransitionToSelf { get; }
        public Func<EventArgs> FuncArgs { get; }

        public Transition(IState to, IPredicate condition, bool canTransitionToSelf = false, Func<EventArgs> funcArgs = null)
        {
            this.To = to;
            this.Condition = condition;
            this.CanTransitionToSelf = canTransitionToSelf;
            this.FuncArgs = funcArgs;
        }
    }
}