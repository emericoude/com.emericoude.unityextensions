namespace Emericoude.StateMachines
{
    public class TriggerPredicate : IPredicate
    {
        public void Trigger() => this.triggered = true;
        private bool triggered = false;

        public bool Evaluate()
        {
            if (!this.triggered) return false;
            this.triggered = false;
            return true;
        }
    }
}