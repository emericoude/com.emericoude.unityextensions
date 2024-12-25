namespace Emericoude.StateMachine
{
    /// <summary> A trigger that will unset itself the first time it is evaluated as true. </summary>
    public class SingleUseTriggerPredicate : IPredicate, ITrigger
    {
        public bool IsTriggered { get; private set; } = false;
        public void Trigger() => this.IsTriggered = true;
        public void ResetTrigger() => this.IsTriggered = false;

        public bool Evaluate()
        {
            if (!this.IsTriggered) return false;
            this.ResetTrigger();
            return true;
        }
    }
}