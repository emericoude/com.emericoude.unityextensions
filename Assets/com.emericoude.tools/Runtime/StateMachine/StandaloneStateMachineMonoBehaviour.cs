using System;
using UnityEngine.Serialization;

namespace Emericoude.StateMachine
{
    public class StandaloneStateMachineMonoBehaviour : StateMachineMonoBehaviour
    {
        public bool autoEnableAndInitOnStart;

        protected virtual void Start() {
            if (this.autoEnableAndInitOnStart) {
                this.SetActiveAndEnabled(true);
                this.OnInit();
                this.OnEnter();
            }
        }

        protected virtual void Update() {
            if (this.IsActive) this.OnUpdate();
        }

        protected virtual void FixedUpdate() {
            if (this.IsActive) this.OnFixedUpdate();
        }

        protected void OnDisable() {
            if (this.IsActive) this.OnExit();
        }

        protected virtual void OnDestroy() {
            if (this.IsActive) this.OnExit();
        }
    }
}