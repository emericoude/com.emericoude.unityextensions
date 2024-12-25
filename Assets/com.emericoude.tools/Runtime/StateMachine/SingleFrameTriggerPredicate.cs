namespace Emericoude.StateMachine
{
    #if CYSHARP_UNITASK
    using Cysharp.Threading.Tasks;
    
    /// <summary> A trigger that will unset itself on the next frame, if it is evaluated as true. </summary>
    /// <remarks> REQUIRES UNITASK. </remarks>
    public class SingleFrameTriggerPredicate : IPredicate, ITrigger
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
            resetTask ??= ResetTriggerNextFrame();
            
            return true;
        }

        private async UniTask ResetTriggerNextFrame()
        {
            await UniTask.NextFrame();
            this.ResetTrigger();
        }
    }
    #endif
}