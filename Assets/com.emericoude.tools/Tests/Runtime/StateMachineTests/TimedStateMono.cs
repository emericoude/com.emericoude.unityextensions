using Emericoude.StateMachine;
using UnityEngine;
using UnityEngine.Serialization;

namespace Emericoude.Tests
{
    public class TimedStateMono : StateMonoBehaviour
    {
        public bool IsTimerExpired() => this.timer <= 0f;
        
        [SerializeField] private float duration = 2f;
        private float timer;

        public override void OnEnter()
        {
            base.OnEnter();
            this.timer = duration;
        }

        public override void OnUpdate()
        {
            this.timer -= Time.deltaTime;
        }

        public override void OnExit()
        {
            base.OnExit();
            this.timer = duration;
        }
    }
}