using Cysharp.Threading.Tasks;

namespace Emericoude.StateMachine
{
    /// <summary> A trigger that will unset itself on the next frame, after being evaluated as true once. </summary>
    /// <remarks>
    /// As opposed to <see cref="SingleUseTriggerPredicate"/>, this can be Evaluated as true multiple times in the frame.
    /// As opposed to <see cref="SingleFrameTriggerPredicate"/>, this will only start unsetting once it has been evaluated as true once.
    /// </remarks>
    public class SharedFrameTriggerPredicate : IPredicate, ITrigger
    {
        public bool IsTriggered { get; private set; }
        
        public void Trigger() => this.IsTriggered = true;
        public void ResetTrigger() {
            this.IsTriggered = false;
            this.resetTask = null;
        }
        
        private UniTask? resetTask;

        public bool Evaluate()
        {
            if (!this.IsTriggered) return false;
            this.resetTask ??= this.ResetTriggerNextFrame();
            
            return true;
        }

        private async UniTask ResetTriggerNextFrame()
        {
            await UniTask.NextFrame();
            this.ResetTrigger();
        }
    }
}