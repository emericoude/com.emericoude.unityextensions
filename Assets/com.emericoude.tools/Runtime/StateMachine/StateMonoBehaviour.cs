using System;
using UnityEngine;

namespace Emericoude.StateMachine
{
    public abstract class StateMonoBehaviour : MonoBehaviour, IState
    {
        public bool IsActive { get; private set; } = false;

        public virtual void OnInit()
        {
            this.enabled = this.IsActive;
        }

        public virtual void OnEnter(EventArgs args = null)
        {
            this.SetActiveAndEnabled(true);
        }
        
        public virtual void OnUpdate() { }
        public virtual void OnFixedUpdate() { }

        public virtual void OnExit()
        {
            this.SetActiveAndEnabled(false);
        }

        protected void SetActiveAndEnabled(bool value)
        {
            this.IsActive = value;
            this.enabled = value;
        }
    }
}