using System;

namespace Emericoude.StateMachine
{
    /// <summary> Interface definition for a state machine's transition. </summary>
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
        Func<EventArgs> FuncArgs { get; }
    }
}