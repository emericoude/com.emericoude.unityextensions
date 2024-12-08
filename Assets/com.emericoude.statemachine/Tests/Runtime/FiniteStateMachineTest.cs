using UnityEngine;

namespace Emericoude.StateMachines.Tests
{
    [RequireComponent(typeof(FirstStateMonoTest), typeof(SecondStateMonoTest))]
    public class FiniteStateMachineTest : MonoBehaviour
    {
        [SerializeField] private float stateDuration = 1f;
        [SerializeField] private TimedStateMono firstMonoState;
        [SerializeField] private TimedStateMono secondMonoState;
        
        private FiniteStateMachine stateMachine;
        private FiniteStateMachine stateMachineUsingSerializedStates;

        private void Awake()
        {
            this.stateMachine = new FiniteStateMachine();
            this.stateMachine.OnStateChanged += this.OnStateChanged;
            var firstTimedState = new FirstStateTest(this.stateDuration);
            var secondTimedState = new SecondStateTest(this.stateDuration);
            this.stateMachine.AddTransition(firstTimedState, secondTimedState, new FuncPredicate(() => firstTimedState.IsTimerExpired()));
            this.stateMachine.AddTransition(secondTimedState, firstTimedState, new FuncPredicate(() => secondTimedState.IsTimerExpired()));
            this.stateMachine.SetState(firstTimedState);

            this.stateMachineUsingSerializedStates = new FiniteStateMachine();
            this.stateMachineUsingSerializedStates.OnStateChanged += this.OnStateChanged;
            this.stateMachineUsingSerializedStates.AddTransition(this.firstMonoState, this.secondMonoState, new FuncPredicate(() => this.firstMonoState.IsTimerExpired()));
            this.stateMachineUsingSerializedStates.AddTransition(this.secondMonoState, this.firstMonoState, new FuncPredicate(() => this.secondMonoState.IsTimerExpired()));
            this.stateMachineUsingSerializedStates.SetState(this.firstMonoState);
        }

        private void Update()
        {
            this.stateMachine.Update();
            this.stateMachineUsingSerializedStates.Update();
        }

        private void FixedUpdate()
        {
            this.stateMachine.FixedUpdate();
            this.stateMachineUsingSerializedStates.FixedUpdate();
        }

        private void OnStateChanged(IState from, IState to)
        {
            Debug.Log($"State changed from: {from}, to: {to}.");
        }
    }
}
