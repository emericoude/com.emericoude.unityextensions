namespace Emericoude.StateMachines
{
    /// <summary> Interface definition for a predicate (i.e. something that returns true or false based on some logic). </summary>
    public interface IPredicate
    {
        public bool Evaluate();
    }
}