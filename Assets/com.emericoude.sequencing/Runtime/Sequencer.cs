using System;
using System.Collections.Generic;

using UnityEngine;

namespace Emericoude.Gameplay.Sequencing
{
    /// <summary>
    /// Handles a sequence of step through a queue. <para/>
    /// 
    /// Note that, by default, steps commencing and concluding can be handled by either the step manager or the steps themselves.
    /// Since a mix of both is desired, it is vital that this loop is properly handled so that it cannot loop indefinitely.
    /// As of now, steps will notify this manager when they begin and when they conclude for the flow to be handled. <para/>
    /// 
    /// This should provide a seamless implementation, where you can either call <see cref="BeginNextStep"/> here,
    /// or call <see cref="SequenceStep"/>.Begin() and <see cref="SequenceStep"/>.Conclude() manually (or from within a step itself). <para/>
    ///
    /// As an example, the <see cref="TimedSequenceStep"/> will conclude itself when its timer expires.
    /// </summary>
    public class Sequencer : MonoBehaviour
    {
        /// <summary> Handles a change in step. </summary>
        /// <param name="previous"> The previous step. Null if there are none. </param>
        /// <param name="current"> The current (newly begind) step. </param>
        public delegate void StepChangedEventHandler(SequenceStep previous, SequenceStep current);
        
        /// <summary> Invoked commencing the first step (when we have no <see cref="PreviousStep"/>). </summary>
        public event Action OnSequenceBegun;

        /// <summary> Invoked when a new step begins. </summary>
        public event StepChangedEventHandler OnStepChanged;
        
        /// <summary> Invoked when we empty our queue. </summary>
        /// <remarks> Still called if <see cref="loopSequence"/> is true, before we refill the queue. </remarks>
        public event Action OnSequenceExhausted;
        
        /// <summary> The <see cref="sequence"/>'s steps, represented in an active queue. </summary>
        [DrawInDebugInfoBox]
        public Queue<SequenceStep> StepQueue { get; protected set; }
        
        /// <summary> The step at the top of the <see cref="StepQueue"/>. </summary>
        /// <remarks> Can be null if the queue is null or empty. </remarks>
        [DrawInDebugInfoBox]
        public SequenceStep CurrentStep => StepQueue?.Peek();
        
        /// <summary> The previous step. </summary>
        /// <remarks> Can be null if no step has concluded yet. </remarks>
        [DrawInDebugInfoBox]
        public SequenceStep PreviousStep { get; protected set; }

        /// <summary> Whether the sequencer is currently in progress. </summary>
        public bool IsInProgress => CurrentStep is { State: StepState.Ongoing };

        [Header("Settings: Steps")] 
        [Tooltip("The step sequence for this step manager.")]
        [SerializeField] protected Sequence sequence;
        
        protected virtual void Awake()
        {
            if (sequence)
            {
                InitializeQueue();
            }
        }

        protected virtual void Start()
        {
            if (sequence && sequence.BeginOnStart)
            {
                if (StepQueue.Count <= 0)
                {
                    Debug.LogWarning("Step queue is empty, cannot begin.");
                    return;
                }
                
                BeginNextStep();
            }
        }

        protected void Update()
        {
            if (StepQueue.TryPeek(out SequenceStep stepAtTopOfQueue))
            {
                if (stepAtTopOfQueue.State == StepState.Ongoing)
                {
                    stepAtTopOfQueue.Update();
                }
            }
        }

        /// <summary> Creates the queue and initializes its step via Awake. </summary>
        protected virtual void InitializeQueue()
        {
            StepQueue = new Queue<SequenceStep>(sequence.CopySequence());
            foreach (var step in StepQueue)
            {
                step.Awake(this);
            }
        }

        /// <summary> Concludes the current step and begins the next one. Use this to begin the sequence as well. </summary>
        /// <remarks> This may run multiple times if it needs to conclude the current step. </remarks>
        public virtual void BeginNextStep()
        {
            if (StepQueue.Count < 0)
            {
                Debug.LogWarning("Sequencer has no step queued, and cannot begin next step.");
                return;
            }
            
            if (StepQueue.TryPeek(out SequenceStep stepAtTopOfQueue))
            {
                if (stepAtTopOfQueue.State == StepState.Queued)
                {
                    stepAtTopOfQueue.Begin();
                    if (PreviousStep == null) OnSequenceBegun?.Invoke();
                    OnStepChanged?.Invoke(PreviousStep, stepAtTopOfQueue);
                }
                else
                {
                    stepAtTopOfQueue.Conclude();
                }
            }
            else
            {
                OnSequenceExhausted?.Invoke();
                if (sequence.Loops)
                {
                    InitializeQueue();
                    BeginNextStep();
                }
            }
        }

        /// <summary> Called by a <see cref="SequenceStep"/> when it is commencing. </summary>
        /// <remarks>
        /// - If the step is not queued, nothing will happen. <br/>
        /// - If the step is not the first in the queue, all previous step will be dequeued without going through a normal flow, effectively being skipped.
        /// </remarks>
        /// <param name="sequenceStep"> The step that is commencing. </param>
        internal void NotifyOfStepBeginning(SequenceStep sequenceStep)
        {
            if (sequenceStep.State != StepState.Queued) return;
            
            while (CurrentStep != sequenceStep)
            {
                StepQueue.Dequeue();
            }
            
            BeginNextStep();
        }

        /// <summary> Called by a <see cref="SequenceStep"/> when it is concluding. </summary>
        /// <remarks>
        /// - If the step is not at the top of the queue, nothing will happen. <br/>
        /// </remarks>
        /// <param name="sequenceStep"> The step that is concluding. </param>
        internal void NotifyOfStepConcluding(SequenceStep sequenceStep)
        {
            if (StepQueue.TryPeek(out SequenceStep stepAtTopOfQueue))
            {
                if (stepAtTopOfQueue != sequenceStep) return;
                
                PreviousStep = sequenceStep;
                StepQueue.Dequeue();
                BeginNextStep();
            }
        }

        /// <summary> Sets the sequence to a new sequence. If there is already one active, we conclude its current step before assigning the new sequence. </summary>
        /// <param name="sequence"> The sequence to assign. </param>
        public void SetSequence(Sequence sequence)
        {
            //force the step to conclude
            if (sequence != null && IsInProgress)
            {
                CurrentStep.Conclude(); 
            }

            this.sequence = sequence;
            InitializeQueue();
        }
    }
}
