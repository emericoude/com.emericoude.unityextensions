using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;

namespace Emericoude.Gameplay.Rounds
{
    public class RoundManager : MonoBehaviour
    {
        [Header("Settings")] 
        [SerializeField] protected bool beginOnStart = true;

        [Header("Settings: Rounds")] 
        [SerializeField] protected List<Round> rounds = new List<Round>();
        
        public Round CurrentRound => RoundQueue.Peek();
        public Round PreviousRound { get; protected set; }
        public Queue<Round> RoundQueue { get; protected set; }
        
        protected virtual void Awake()
        {
            RoundQueue = new Queue<Round>(rounds);
        }

        protected virtual void Start()
        {
            if (beginOnStart && rounds.Count > 0)
            {
                CommenceNextRound();
            }
        }

        protected virtual void CommenceNextRound()
        {
            if (RoundQueue.TryPeek(out Round roundAtTopOfQueue))
            {
                if (roundAtTopOfQueue.Status == Round.RoundStatus.Queued)
                {
                    roundAtTopOfQueue.Commence();
                }
                else if (roundAtTopOfQueue.Status == Round.RoundStatus.Ongoing)
                {
                    roundAtTopOfQueue.Conclude();
                    PreviousRound = roundAtTopOfQueue;
                    RoundQueue.Dequeue();
                    
                    CommenceNextRound(); //run it again
                }
            }
            else
            {
                Debug.LogWarning("Queue is out of rounds, cannot commence next round.");
            }
        }
    }
}
