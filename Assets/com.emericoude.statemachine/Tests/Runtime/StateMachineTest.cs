using Sirenix.OdinInspector;
using UnityEngine;

namespace Emericoude.StateMachines.Tests
{
    [RequireComponent(typeof(FirstStateMonoTest), typeof(SecondStateMonoTest))]
    public class StateMachineTest : MonoBehaviour
    {
        [SerializeField] private bool testFiniteSFM = true;
        [SerializeField] private bool testFiniteSFMSerialized = true;
        [SerializeField] private bool testHierarchicalSFM = true;
        [SerializeField] private float stateDuration = 1f;
        [SerializeField] private TimedStateMono firstMonoState;
        [SerializeField] private TimedStateMono secondMonoState;
        
        private StateMachine stateMachine;
        private StateMachine stateMachineUsingSerializedStates;
        private StateMachine hierarchicalStateMachine;

        private bool wasSwapInvokedThisFrame = false;

        private void Awake()
        {
            if (this.testFiniteSFM)
            {
                this.stateMachine = new StateMachine();
                this.stateMachine.OnStateChanged += this.OnStateChanged;
                var firstTimedState = new FirstStateTest(this.stateDuration);
                var secondTimedState = new SecondStateTest(this.stateDuration);
                this.stateMachine.AddTransition(firstTimedState, secondTimedState, new FuncPredicate(() => firstTimedState.IsTimerExpired()));
                this.stateMachine.AddTransition(secondTimedState, firstTimedState, new FuncPredicate(() => secondTimedState.IsTimerExpired()));
                this.stateMachine.SetState(firstTimedState);
            }
            
            if (this.testFiniteSFMSerialized)
            {
                this.stateMachineUsingSerializedStates = new StateMachine();
                this.stateMachineUsingSerializedStates.OnStateChanged += this.OnStateChanged;
                this.stateMachineUsingSerializedStates.AddTransition(this.firstMonoState, this.secondMonoState, new FuncPredicate(() => this.firstMonoState.IsTimerExpired()));
                this.stateMachineUsingSerializedStates.AddTransition(this.secondMonoState, this.firstMonoState, new FuncPredicate(() => this.secondMonoState.IsTimerExpired()));
                this.stateMachineUsingSerializedStates.SetState(this.firstMonoState);
            }
            
            if (this.testHierarchicalSFM)
            {
                this.hierarchicalStateMachine = new StateMachine();
                
                var firstParentState = new FirstParentState();
                var firstParentChildOne = new FirstStateTest(this.stateDuration);
                var firstParentChildTwo = new SecondStateTest(this.stateDuration);
                
                var secondParentState = new SecondParentState();
                var secondParentChildOne = new FirstStateTest(this.stateDuration);
                var secondParentChildTwo = new SecondStateTest(this.stateDuration);
                
                this.hierarchicalStateMachine.OnStateChanged += this.OnStateChanged;
                this.hierarchicalStateMachine.AddTransition(firstParentState, secondParentState, new FuncPredicate(() => this.wasSwapInvokedThisFrame));
                this.hierarchicalStateMachine.AddTransition(secondParentState, firstParentState, new FuncPredicate(() => this.wasSwapInvokedThisFrame));
                
                firstParentState.OnStateChanged += this.OnStateChanged;
                firstParentState.AddTransition(firstParentChildOne, firstParentChildTwo, new FuncPredicate(() => firstParentChildOne.IsTimerExpired()));
                firstParentState.AddTransition(firstParentChildTwo, firstParentChildOne, new FuncPredicate(() => firstParentChildTwo.IsTimerExpired()));
                
                secondParentState.OnStateChanged += this.OnStateChanged;
                secondParentState.AddTransition(secondParentChildOne, secondParentChildTwo, new FuncPredicate(() => secondParentChildOne.IsTimerExpired()));
                secondParentState.AddTransition(secondParentChildTwo, secondParentChildOne, new FuncPredicate(() => secondParentChildTwo.IsTimerExpired()));
                
                firstParentState.SetState(firstParentChildOne);
                secondParentState.SetState(secondParentChildOne);
                this.hierarchicalStateMachine.SetState(firstParentState);
            }
        }

        private void Update()
        {
            this.stateMachine?.OnUpdate();
            this.stateMachineUsingSerializedStates?.OnUpdate();
            this.hierarchicalStateMachine?.OnUpdate();
            this.wasSwapInvokedThisFrame = false;
        }

        private void FixedUpdate()
        {
            this.stateMachine?.OnFixedUpdate();
            this.stateMachineUsingSerializedStates?.OnFixedUpdate();
            this.hierarchicalStateMachine?.OnFixedUpdate();
        }

        private void OnStateChanged(IState from, IState to)
        {
            Debug.Log($"State changed from: {from}, to: {to}.");
        }

        [Button("Swap hierarchical state")]
        private void OnContextMenuButtonClick()
        {
            this.wasSwapInvokedThisFrame = true;
        }
    }
}
