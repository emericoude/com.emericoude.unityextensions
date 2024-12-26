namespace Emericoude.StateMachine
{
    public abstract class StateMachineMonoBehaviour : StateMonoBehaviour
    {
        public StateMachine StateMachine { get; } = new();

        public override void OnInit()
        {
            this.StateMachine.OnInit();
            this.enabled = this.IsActive;
        }

        public override void OnEnter()
        {
            this.SetActiveAndEnabled(true);
            this.StateMachine.OnEnter();
        }
        
        public override void OnUpdate() => this.StateMachine.OnUpdate();
        public override void OnFixedUpdate() => this.StateMachine.OnFixedUpdate();

        public override void OnExit()
        {
            this.StateMachine.OnExit();
            this.SetActiveAndEnabled(false);
        }
    }
}