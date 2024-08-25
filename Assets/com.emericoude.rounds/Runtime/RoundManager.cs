using System;
using System.Collections.Generic;

using UnityEngine;

namespace Emericoude.Gameplay.Rounds
{
    /// <summary>
    /// Handles a sequence of round through a queue. <para/>
    /// 
    /// Note that, by default, rounds commencing and concluding can be handled by either the round manager or the rounds themselves.
    /// Since a mix of both is desired, it is vital that this loop is properly handled so that it cannot loop indefinitely.
    /// As of now, rounds will notify this manager when they commence and when they conclude for the flow to be handled. <para/>
    /// 
    /// This should provide a seamless implementation, where you can either call <see cref="CommenceNextRound"/> here,
    /// or call <see cref="Round"/>.Commence() and <see cref="Round"/>.Conclude() manually (or from within a round itself). <para/>
    ///
    /// As an example, the <see cref="TimedRound"/> will conclude itself when its timer expires.
    /// </summary>
    public class RoundManager : MonoBehaviour
    {
        /// <summary> Handles a change in round. </summary>
        /// <param name="previous"> The previous round. Null if there are none. </param>
        /// <param name="current"> The current (newly commenced) round. </param>
        public delegate void RoundChangedEventHandler(Round previous, Round current);
        
        /// <summary> Invoked commencing the first round (when we have no <see cref="PreviousRound"/>). </summary>
        public event Action OnRoundSequenceCommenced;

        /// <summary> Invoked when a new round commences. </summary>
        public event RoundChangedEventHandler OnRoundChanged;
        
        /// <summary> Invoked when we empty our queue. </summary>
        /// <remarks> Still called if <see cref="loopSequence"/> is true, before we refill the queue. </remarks>
        public event Action OnRoundSequenceExhausted;
        
        /// <summary> The <see cref="roundSequenceSettings"/>'s rounds, represented in an active queue. </summary>
        [DrawInDebugInfoBox]
        public Queue<Round> RoundQueue { get; protected set; }
        
        /// <summary> The round at the top of the <see cref="RoundQueue"/>. </summary>
        /// <remarks> Can be null if the queue is null or empty. </remarks>
        [DrawInDebugInfoBox]
        public Round CurrentRound => RoundQueue?.Peek();
        
        /// <summary> The previous round. </summary>
        /// <remarks> Can be null if no round has concluded yet. </remarks>
        [DrawInDebugInfoBox]
        public Round PreviousRound { get; protected set; }

        [Header("Settings: Rounds")] 
        [Tooltip("The round sequence for this round manager.")]
        public RoundSequence roundSequenceSettings;
        
        protected virtual void Awake()
        {
            SetupQueue();
        }

        protected virtual void Start()
        {
            if (roundSequenceSettings && roundSequenceSettings.CommenceOnStart)
            {
                if (RoundQueue.Count <= 0)
                {
                    Debug.LogWarning("Round queue is empty, cannot commence.");
                    return;
                }
                
                CommenceNextRound();
            }
        }

        protected void Update()
        {
            if (RoundQueue.TryPeek(out Round roundAtTopOfQueue))
            {
                if (roundAtTopOfQueue.State == RoundState.Ongoing)
                {
                    roundAtTopOfQueue.Update();
                }
            }
        }

        /// <summary> Creates the queue and initializes its round via Awake. </summary>
        protected virtual void SetupQueue()
        {
            RoundQueue = new Queue<Round>(roundSequenceSettings.CopySequence());
            foreach (var round in RoundQueue)
            {
                round.Awake(this);
            }
        }

        /// <summary> Concludes the current round and begins the next one. Use this to begin the sequence as well. </summary>
        /// <remarks> This may run multiple times if it needs to conclude the current round. </remarks>
        public virtual void CommenceNextRound()
        {
            if (RoundQueue.Count < 0)
            {
                Debug.LogWarning("Round manager has no rounds queued, and cannot commence next round.");
                return;
            }
            
            if (RoundQueue.TryPeek(out Round roundAtTopOfQueue))
            {
                if (roundAtTopOfQueue.State == RoundState.Queued)
                {
                    roundAtTopOfQueue.Commence();
                    if (PreviousRound == null) OnRoundSequenceCommenced?.Invoke();
                    OnRoundChanged?.Invoke(PreviousRound, roundAtTopOfQueue);
                }
                else
                {
                    roundAtTopOfQueue.Conclude();
                }
            }
            else
            {
                OnRoundSequenceExhausted?.Invoke();
                if (roundSequenceSettings.Loops)
                {
                    SetupQueue();
                    CommenceNextRound();
                }
            }
        }

        /// <summary> Called by a <see cref="Round"/> when it is commencing. </summary>
        /// <remarks>
        /// - If the round is not queued, nothing will happen. <br/>
        /// - If the round is not the first in the queue, all previous rounds will be dequeued without going through a normal flow, effectively being skipped.
        /// </remarks>
        /// <param name="round"> The round that is commencing. </param>
        public void NotifyOfRoundCommencing(Round round)
        {
            if (round.State != RoundState.Queued) return;
            
            while (CurrentRound != round)
            {
                RoundQueue.Dequeue();
            }
            
            CommenceNextRound();
        }

        /// <summary> Called by a <see cref="Round"/> when it is concluding. </summary>
        /// <remarks>
        /// - If the round is not at the top of the queue, nothing will happen. <br/>
        /// </remarks>
        /// <param name="round"> The round that is concluding. </param>
        public void NotifyOfRoundConclusion(Round round)
        {
            if (RoundQueue.TryPeek(out Round roundAtTopOfQueue))
            {
                if (roundAtTopOfQueue != round) return;
                
                PreviousRound = round;
                RoundQueue.Dequeue();
                CommenceNextRound();
            }
        }
    }
}
