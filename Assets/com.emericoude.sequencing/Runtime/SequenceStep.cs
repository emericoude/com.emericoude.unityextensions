using System;

using UnityEngine;

namespace Emericoude.Gameplay.Sequencing
{
    /// <summary> A round's state. </summary>
    public enum StepState
    {
        /// <summary> In queue, waiting to be started. </summary>
        Queued,
        /// <summary> Active, in progress. </summary>
        Ongoing,
        /// <summary> Temporarily disabled. </summary>
        Paused,
        /// <summary> Finished, completed. </summary>
        Concluded
    }
    
    /// <summary> A specific state, phase or moment during your game's flow. You can implement how a round flows and function by inheriting it. Look at <see cref="TimedSequenceStep"/> as a example. </summary>
    /// <remarks> As per this implementation, only one round per <see cref="Sequencer"/> should be active at a time. </remarks>
    [Serializable]
    public abstract class SequenceStep
    {
        /// <summary> Invoked when the round begins. </summary>
        public event Action OnBegun;
        
        /// <summary> Invoked when the round ends. </summary>
        public event Action OnConcluded;
        
        /// <summary> Invoked when the round is paused. </summary>
        public event Action OnPaused;
        
        /// <summary> Invoked when the round is unpaused. </summary>
        public event Action OnResumed;
        
        /// <summary> The <see cref="Sequencer"/> owning this round. </summary>
        public Sequencer Sequencer { get; protected set; }
        
        /// <summary> This round's state. </summary>
        [DrawInDebugInfoBox]
        public StepState State { get; protected set; } = StepState.Queued;

        /// <summary> Defines how your round type is copied over. Used for runtime. </summary>
        /// <remarks> IT IS PRIMORDIAL TO IMPLEMENT THIS SO THAT ALL SERIALIZED SETTINGS ARE COPIED AT RUNTIME. </remarks>
        /// <returns> An exact clone of this round. Used for runtime. </returns>
        public abstract SequenceStep Clone();

        /// <summary> Use this to initialize anything that should be initialized before start. </summary>
        /// <param name="sequencer"> The round manager handling this round. </param>
        public virtual void Awake(Sequencer sequencer)
        {
            Sequencer = sequencer;
        }
        
        /// <summary> Begins (starts, begins) the round. </summary>
        /// <remarks> By default, this will notify the round manager of its commencing, potentially skipping previously queued rounds. </remarks>
        public virtual void Begin()
        {
            State = StepState.Ongoing;
            Sequencer.NotifyOfStepBeginning(this);
            OnBegun?.Invoke();
        }

        /// <summary> Update tick that is used if the state is ongoing. </summary>
        /// <remarks> ONLY RUNS FOR AN ONGOING STATE. </remarks>
        //for now only runs for ongoing state, but it might be better to allow for state management by the user.
        public virtual void Update() 
        {
            
        }

        /// <summary> Concludes (ends, completes, finishes) the round. </summary>
        /// <remarks> By default, this will notify the round manager of its conclusion so the next round can be started. </remarks>
        public virtual void Conclude()
        {
            State = StepState.Concluded;
            Sequencer.NotifyOfStepConcluding(this);
            OnConcluded?.Invoke();
        }

        /// <summary> Pauses the round if it is ongoing. </summary>
        public virtual void Pause()
        {
            if (State != StepState.Ongoing)
            {
                Debug.LogWarning($"Cannot pause a round that is {State}.");
                return;
            }

            State = StepState.Paused;
            OnPaused?.Invoke();
        }

        /// <summary> Unpauses the round if it is paused. </summary>
        public virtual void Resume()
        {
            if (State != StepState.Paused)
            {
                Debug.LogWarning($"Cannot resume a round that is {State}.");
                return;
            }
            
            State = StepState.Ongoing;
            OnResumed?.Invoke();
        }
    }
}
