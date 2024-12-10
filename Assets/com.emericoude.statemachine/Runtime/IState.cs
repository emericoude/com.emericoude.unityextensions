namespace Emericoude.StateMachines
{
    public interface IState
    {
        /// <summary> Called whenever the state is entered. </summary>
        public void OnEnter();
        /// <summary> Called every frame if the state is the active one. </summary>
        public void Update();
        /// <summary> Called every physics frame if the state is the active one. </summary>
        public void FixedUpdate();
        /// <summary> Called whenever the state is exited. </summary>
        public void OnExit();
    }
}