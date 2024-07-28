using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Emericoude.Gameplay.Rounds
{
    /// <summary> A round with a timer attached to it, concluding itself once it is expired. </summary>
    public class TimedRound : Round
    {
#if ODIN_INSPECTOR
        [SuffixLabel("seconds"), MinValue(0f)]
#endif
        [Tooltip("This round's duration in seconds.")]
        public float Duration = 60;

        /// <summary> This round's timer. </summary>
        /// <remarks> Only counted down if the round is ongoing. </remarks>
        public float Timer { get; set; }

        /// <summary> Can be used to speed up or slow down the timer. </summary>
        public float TimerSpeedModifier { get; set; } = 1f;

        public override void Commence()
        {
            //keep base.Commence() in your implementations, so that the flow is properly handled by the round manager
            base.Commence(); 

            Timer = Duration;
        }

        public override void Update()
        {
            base.Update();

            Timer -= Time.deltaTime * TimerSpeedModifier;
            if (Timer <= 0f)
            {
                Conclude();
            }   
        }

        public override void Conclude()
        {
            // We set the timer to 0 to represent its conclusion, let's say it's used in UI.
            // Since the round might not always be concluded via its timer (e.g. skipped or other condition), 
            // it's good practice to clean things up in the Conclude function.
            Timer = 0f;
            
            //Call base.Conclude() when you're ready for the flow to be handled by the manager.
            base.Conclude();
        }
    }
}