namespace Emericoude.StateMachines
{
    /// <summary> Interface definition for a state machine's transition. </summary>
    public interface ITransition
    {
        IState To { get; }
        IPredicate Condition { get; }
    }
}