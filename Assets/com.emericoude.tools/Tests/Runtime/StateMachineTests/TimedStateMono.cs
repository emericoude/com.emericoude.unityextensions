using System;

using Emericoude.StateMachine;

using UnityEngine;
using Random = UnityEngine.Random;

namespace Emericoude.Tests
{
    public class TimedStateMono : StateMonoBehaviour
    {
        public bool IsTimerExpired() => this.timer <= 0f;
        
        [SerializeField] private float duration = 2f;
        [SerializeField] private int randomNumberFromArgs;
        private float timer;

        public override void OnEnter(EventArgs args = null)
        {
            base.OnEnter(args);
            this.timer = duration;
            
            if (args is TestEventArg testEventArg) {
                this.randomNumberFromArgs = testEventArg.RandomNumber;
            }
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

        public TestEventArg GetRandomNumber() {
            return new TestEventArg(Random.Range(0, 100));
        }

        public class TestEventArg : EventArgs
        {
            public int RandomNumber;
            
            public TestEventArg(int randomNumber) => this.RandomNumber = randomNumber;
        }
    }
}