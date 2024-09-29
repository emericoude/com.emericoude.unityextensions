using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Emericoude.Gameplay.Sequencing
{
    /// <summary> A preset setting representing a sequence (series, set, group, queue) of steps. </summary>
    [CreateAssetMenu(fileName = "New Sequence", menuName = "Emericoude/Sequencing/Sequence")]
    public class Sequence : ScriptableObject
    {
        [Header("Settings")]
        [Tooltip("If true, a sequencer referencing this on start will begin the sequence. Disable if you want to manually begin the sequence using CommenceNextRound.")]
        [SerializeField] protected bool beginOnStart = true;
        
        [Tooltip("If true, the sequence will be looped after it has been exhausted.")]
        [SerializeField] protected bool loopSequence = true;
        
        [Header("Sequence Steps")]
        #if !ODIN_INSPECTOR
        [TypeFilter(typeof(SequenceStep))]
        #endif
        [Tooltip("A set of steps, used in a queue by default.")]
        [SerializeReference] protected List<SequenceStep> sequence = new List<SequenceStep>();
        
        /// <summary> Whether to auto-commence the sequence on start. </summary>
        public bool BeginOnStart => beginOnStart;
        
        /// <summary> Whether to loop the sequence on when it exhausts all its steps. </summary>
        public bool Loops => loopSequence;

        /// <summary> Copies each step to a new sequence. Used so we don't override values at runtime. </summary>
        /// <remarks> You must implement your step's Clone function for this to work properly. </remarks>
        /// <returns> A collection of steps, copied from <see cref="sequence"/>. </returns>
        public IEnumerable<SequenceStep> CopySequence()
        {
            return sequence.Select(step => step.Clone());
        }
    }
}
