using System.ComponentModel;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Emericoude.Inputs
{
    /// <summary>
    /// A <see cref="HoldInteraction"/> with a maximum duration. The interaction is cancelled upon reaching its maximum duration. <br/>
    /// - Started: When the input has been actuated. <br/>
    /// - Performed: When and after the <see cref="EntryDuration"/> has been reached. <br/>
    /// - Canceled: If the input was released, or the <see cref="ExitDuration"/> has been reached.
    /// </summary>
    #if UNITY_EDITOR
    [InitializeOnLoad]
    #endif
    [DisplayName("Hold With Maximum Duration")]
    public class TimedHoldInteraction : IInputInteraction
    {
        /// <remarks> If this is less than or equal to 0 (the default), <see cref="InputSettings.defaultButtonPressPoint"/> is used instead. </remarks>
        [Range(0.01f, 1f), Tooltip("Magnitude threshold that must be crossed by an actuated control for it to be considered pressed.")]
        public float PressPoint;
        
        /// <remarks> If this is less than or equal to 0 (the default), <see cref="InputSettings.defaultHoldTime"/> is used. </remarks>
        [Tooltip("Duration in seconds that the control must be pressed for the hold to register.")]
#if ODIN_INSPECTOR
        [PropertyRange(0f, "@this.ExitDuration"), SuffixLabel("Seconds")]
#endif
        public float EntryDuration;
        
        [Tooltip("Duration in seconds that the control must be pressed for the hold to cancel from the actuation time.")]
#if ODIN_INSPECTOR
        [PropertyRange("@this.EntryDuration + 0.1f", "@this.EntryPoint + 20f"), SuffixLabel("Seconds")]
#endif
        public float ExitDuration = 2f;

        private double _timePressed;
        private bool _waitingForExit;
        
        private float EntryDurationOrDefault => EntryDuration > 0.0f ? EntryDuration : InputSystem.settings.defaultHoldTime;
        private float PressPointOrDefault => PressPoint > 0.0 ? PressPoint : InputSystem.settings.defaultButtonPressPoint;

        static TimedHoldInteraction()
        {
            InputSystem.RegisterInteraction<TimedHoldInteraction>();
        }

        [RuntimeInitializeOnLoadMethod]
        private static void Initialize() { }

        public void Process(ref InputInteractionContext context)
        {
            //a timer from SetTimeout has expired
            if (context.timerHasExpired) 
            {
                if (_waitingForExit) //exit timeout
                {
                    context.Canceled();
                    return;
                }
                
                //entry timeout
                ActuatePerformTimer(ref context);
                return;
            }

            switch (context.phase)
            {
                case InputActionPhase.Waiting:
                    if (context.ControlIsActuated(PressPointOrDefault))
                    {
                        _timePressed = context.time;

                        context.Started();
                        context.SetTotalTimeoutCompletionTime(ExitDuration); //overrides the total timeout completion
                        context.SetTimeout(EntryDurationOrDefault); //set the first timeout to entry
                    }
                    break;

                case InputActionPhase.Started:
                    // If we've reached our hold time threshold, perform the hold.
                    // We do this regardless of what state the control changed to.
                    if (context.time - _timePressed >= EntryDurationOrDefault)
                    {
                        ActuatePerformTimer(ref context);
                    }
                    if (!context.ControlIsActuated(PressPointOrDefault))
                    {
                        // Control is no longer actuated so we're done.
                        context.Canceled();
                    }
                    break;

                case InputActionPhase.Performed:
                    if (context.time - _timePressed >= ExitDuration)
                    {
                        context.Canceled();
                    }
                    else if (!context.ControlIsActuated(PressPointOrDefault))
                    {
                        context.Canceled();
                    }
                    break;
                
            }
        }

        public void Reset()
        {
            _timePressed = 0f;
            _waitingForExit = false;
        }

        private void ActuatePerformTimer(ref InputInteractionContext context)
        {
            _waitingForExit = true;
            
            context.PerformedAndStayPerformed();
            context.SetTimeout(ExitDuration - EntryDurationOrDefault);
        }
    }
}
