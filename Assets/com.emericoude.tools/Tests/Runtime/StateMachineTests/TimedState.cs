using System;
using Emericoude.StateMachine;
using UnityEngine;

namespace Emericoude.Tests
{
    public class TimedState : IState
    {
        public bool IsTimerExpired() => this.timer <= 0f;
        
        public float Duration;
        private float timer;

        public TimedState(float duration)
        {
            this.Duration = duration;
        }

        public void OnInit()
        {
        }

        public void OnEnter(EventArgs args = null)
        {
            this.timer = this.Duration;
        }

        public void OnUpdate()
        {
            this.timer -= Time.deltaTime;
        }

        public void OnFixedUpdate()
        {
        }

        public void OnExit()
        {
        }
    }
}