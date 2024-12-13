using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emericoude.StateMachines
{
    public abstract class StateMachineMonoBehaviour : MonoBehaviour, IState
    {
        public StateMachine StateMachine { get; } = new();
        public virtual void OnInit() => this.StateMachine.OnInit();
        public virtual void OnEnter() => this.StateMachine.OnEnter();
        public virtual void OnUpdate() => this.StateMachine.OnUpdate();
        public virtual void OnFixedUpdate() => this.StateMachine.OnFixedUpdate();
        public virtual void OnExit() => this.StateMachine.OnExit();
    }
}