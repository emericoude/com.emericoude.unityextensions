using System;
using System.Collections.Generic;
using UnityEngine;

namespace Emericoude.Gameplay.Rounds
{
    [Serializable]
    public class Round
    {
        public enum RoundStatus
        {
            Queued,
            Ongoing,
            Concluded
        }
        
        [Header("Phases")]
        [SerializeField] protected List<Phase> phases = new List<Phase>();
        
        public RoundStatus Status { get; protected set; } = RoundStatus.Queued;

        public virtual void Commence()
        {
            Status = RoundStatus.Ongoing;
        }

        public virtual void Conclude()
        {
            Status = RoundStatus.Concluded;
        }
    }
}
