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
        
        public void OnEnter()
        {
            this.timer = this.Duration;
        }

        public void Update()
        {
            this.timer -= Time.deltaTime;
        }

        public void FixedUpdate()
        {
        }

        public void OnExit()
        {
        }
    }
}