using System;

namespace Emericoude.StateMachine
{
    public interface IState
    {
        /// <summary> Called whenever the state should initialize. </summary>
        public void OnInit();
        
        /// <summary> Called whenever the state is entered. </summary>
        public void OnEnter(EventArgs args = null);
        /// <summary> Called whenever the state is exited. </summary>
        public void OnExit();
        
        /// <summary> Called every frame if the state is the active one. </summary>
        public void OnUpdate();
        /// <summary> Called every physics frame if the state is the active one. </summary>
        public void OnFixedUpdate();
    }
}