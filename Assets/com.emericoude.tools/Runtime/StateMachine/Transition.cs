namespace Emericoude.StateMachine
{
    /// <summary> Concrete implementation of a ITransition used by state machines, which should cover most needs. </summary>
    public class Transition : ITransition
    {
        public IState To { get; }
        public IPredicate Condition { get; }

        public Transition(IState to, IPredicate condition)
        {
            this.To = to;
            this.Condition = condition;
        }
    }
}