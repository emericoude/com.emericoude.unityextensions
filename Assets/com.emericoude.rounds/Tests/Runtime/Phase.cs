using System;
using UnityEngine;

namespace Emericoude.Gameplay.Rounds
{
    [Serializable]
    public abstract class Phase
    {
        public enum PhaseStatus
        {
            Queued,
            Ongoing,
            Concluded
        }

        public PhaseStatus Status { get; protected set; } = PhaseStatus.Queued;

        public virtual void Commence()
        {
            Status = PhaseStatus.Ongoing;
        }

        public virtual void Conclude()
        {
            Status = PhaseStatus.Concluded;
        }
    }
}
