using System;

using UnityEngine;

namespace Emericoude.Gameplay.Rounds
{
    /// <summary> A round's state. </summary>
    public enum RoundState
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
    
    /// <summary> A specific state, phase or moment during your game's flow. You can implement how a round flows and function by inheriting it. Look at <see cref="TimedRound"/> as a example. </summary>
    /// <remarks> As per this implementation, only one round per <see cref="RoundManager"/> should be active at a time. </remarks>
    [Serializable]
    public abstract class Round
    {
        /// <summary> Invoked when the round begins. </summary>
        public event Action OnCommenced;
        
        /// <summary> Invoked when the round ends. </summary>
        public event Action OnConcluded;
        
        /// <summary> Invoked when the round is paused. </summary>
        public event Action OnPaused;
        
        /// <summary> Invoked when the round is unpaused. </summary>
        public event Action OnResumed;
        
        /// <summary> The <see cref="RoundManager"/> owning this round. </summary>
        public RoundManager RoundManager { get; protected set; }
        
        /// <summary> This round's state. </summary>
        public RoundState State { get; protected set; } = RoundState.Queued;

        /// <summary> Use this to initialize anything that should be initialized before start. </summary>
        /// <param name="roundManager"> The round manager handling this round. </param>
        public virtual void Awake(RoundManager roundManager)
        {
            RoundManager = roundManager;
        }
        
        /// <summary> Commences (starts, begins) the round. </summary>
        /// <remarks> By default, this will notify the round manager of its commencing, potentially skipping previously queued rounds. </remarks>
        public virtual void Commence()
        {
            State = RoundState.Ongoing;
            RoundManager.NotifyOfRoundCommencing(this);
            OnCommenced?.Invoke();
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
            State = RoundState.Concluded;
            RoundManager.NotifyOfRoundConclusion(this);
            OnConcluded?.Invoke();
        }

        /// <summary> Pauses the round if it is ongoing. </summary>
        public virtual void Pause()
        {
            if (State != RoundState.Ongoing)
            {
                Debug.LogWarning($"Cannot pause a round that is {State}.");
                return;
            }

            State = RoundState.Paused;
            OnPaused?.Invoke();
        }

        /// <summary> Unpauses the round if it is paused. </summary>
        public virtual void Resume()
        {
            if (State != RoundState.Paused)
            {
                Debug.LogWarning($"Cannot resume a round that is {State}.");
                return;
            }
            
            State = RoundState.Ongoing;
            OnResumed?.Invoke();
        }
    }
}
