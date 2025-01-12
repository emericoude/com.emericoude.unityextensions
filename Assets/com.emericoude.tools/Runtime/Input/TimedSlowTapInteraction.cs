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
    /// A <see cref="SlowTapInteraction"/> with a maximum duration. The interaction is cancelled upon reaching its maximum duration. <br/>
    /// - Started: When the input has been actuated. <br/>
    /// - Performed: If the input was released after the <see cref="EntryDuration"/> and before <see cref="ExitDuration"/>. <br/>
    /// - Canceled: If the input was released before the <see cref="EntryDuration"/>, or if the <see cref="ExitDuration"/> was reached.
    /// </summary>
#if UNITY_EDITOR
    [InitializeOnLoad]
#endif
    [DisplayName("Long Tap With Maximum Duration")]
    public class TimedSlowTapInteraction : IInputInteraction
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

        static TimedSlowTapInteraction()
        {
            InputSystem.RegisterInteraction<TimedSlowTapInteraction>();
        }
        
        [RuntimeInitializeOnLoadMethod]
        private static void Initialize() { }
        
        public void Process(ref InputInteractionContext context)
        {
            if (context.timerHasExpired)
            {
                if (_waitingForExit)
                {
                    context.Canceled();
                    return;
                }

                ActuateMaxDurationTimer(ref context);
                return;
            }

            switch (context.phase)
            {
                case InputActionPhase.Waiting:
                    if (context.ControlIsActuated(PressPointOrDefault))
                    {
                        _timePressed = context.time;

                        context.Started();
                        context.SetTotalTimeoutCompletionTime(ExitDuration);
                        context.SetTimeout(EntryDurationOrDefault);
                    }
                    break;

                case InputActionPhase.Started:
                    if (context.time - _timePressed >= EntryDurationOrDefault)
                    {
                        if (!_waitingForExit)
                        {
                            ActuateMaxDurationTimer(ref context);
                        }

                        if (!context.ControlIsActuated(PressPointOrDefault))
                        {
                            context.Performed();
                            return;
                        }
                    }

                    if (!context.ControlIsActuated(PressPointOrDefault))
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

        private void ActuateMaxDurationTimer(ref InputInteractionContext context)
        {
            _waitingForExit = true;
            context.SetTimeout(ExitDuration - EntryDurationOrDefault);
        }
    }
}
