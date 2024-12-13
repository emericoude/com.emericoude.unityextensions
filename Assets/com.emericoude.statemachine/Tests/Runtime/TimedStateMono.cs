using UnityEngine;

namespace Emericoude.StateMachines.Tests
{
    public class TimedStateMono : MonoBehaviour, IState
    {
        public bool IsTimerExpired() => this.timer <= 0f;
        
        public float Duration;
        private float timer;

        public TimedStateMono(float duration)
        {
            this.Duration = duration;
        }

        public void OnInit()
        {
        }

        public void OnEnter()
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