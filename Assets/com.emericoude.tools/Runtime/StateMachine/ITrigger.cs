namespace Emericoude.StateMachine
{
    internal interface ITrigger
    {
        public bool IsTriggered { get; }
        public void Trigger();
        public void ResetTrigger();
    }
}