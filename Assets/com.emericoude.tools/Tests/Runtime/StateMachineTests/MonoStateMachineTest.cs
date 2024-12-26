using System;
using Emericoude.StateMachine;
using UnityEngine;

namespace Emericoude.Tests
{
    [RequireComponent(typeof(FirstStateMonoTest), typeof(SecondStateMonoTest))]
    public class MonoStateMachineTest : StateMachineMonoBehaviour
    {
        [SerializeField] private FirstStateMonoTest firstMonoState;
        [SerializeField] private SecondStateMonoTest secondMonoState;
        
        private void Awake()
        {
            this.StateMachine.AddTransition(this.firstMonoState, this.secondMonoState, new FuncPredicate(() => this.firstMonoState.IsTimerExpired()));
            this.StateMachine.AddTransition(this.secondMonoState, this.firstMonoState, new FuncPredicate(() => this.secondMonoState.IsTimerExpired()));
            this.StateMachine.SetState(this.firstMonoState);
            this.SetActiveAndEnabled(true);
        }

        private void Update()
        {
            this.OnUpdate();
        }
    }
}
