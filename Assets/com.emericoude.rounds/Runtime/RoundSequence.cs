using System.Collections.Generic;
using UnityEngine;

namespace Emericoude.Gameplay.Rounds
{
    /// <summary> A preset setting representing a sequence (series, set, group, queue) of rounds. </summary>
    [CreateAssetMenu(fileName = "New Round Sequence", menuName = "Emericoude/Rounds/Round Sequence")]
    public class RoundSequence : ScriptableObject
    {
        [Header("Settings")]
        [Tooltip("If true, a round manager referencing this on start will begin the sequence. Disable if you want to manually begin the sequence using CommenceNextRound.")]
        [SerializeField] protected bool beginOnStart = true;
        
        [Tooltip("If true, the sequence will be looped after it has been exhausted.")]
        [SerializeField] protected bool loopSequence = true;
        
        [Header("Rounds")]
        #if !ODIN_INSPECTOR
        [TypeFilter(typeof(Round))]
        #endif
        [Tooltip("A set of rounds, used in a queue by default.")]
        [SerializeReference] protected List<Round> sequence = new List<Round>();
        
        /// <summary> The set of rounds for this sequence. </summary>
        public List<Round> Sequence => sequence;
        
        /// <summary> Whether to auto-commence the sequence on start. </summary>
        public bool CommenceOnStart => beginOnStart;
        
        /// <summary> Whether to loop the sequence on when it exhausts all its rounds. </summary>
        public bool Loops => loopSequence;
    }
}
