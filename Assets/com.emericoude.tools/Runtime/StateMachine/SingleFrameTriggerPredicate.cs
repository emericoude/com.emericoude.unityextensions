namespace Emericoude.StateMachine
{
    #if CYSHARP_UNITASK
    using Cysharp.Threading.Tasks;
    
    /// <summary> A trigger that will unset itself on the next frame. </summary>
    /// <remarks> REQUIRES UNITASK. </remarks>
    public class SingleFrameTriggerPredicate : IPredicate, ITrigger
    {
        public bool IsTriggered { get; private set; }
        private UniTask? resetTask;
        
        public void Trigger() => this.Trigger(PlayerLoopTiming.Update);
        public void Trigger(PlayerLoopTiming timing) {
            if (this.IsTriggered) return;
            this.IsTriggered = true;
            this.resetTask = this.ResetTriggerNextFrame(timing);
        }
        
        public void ResetTrigger() {
            this.IsTriggered = false;
            this.resetTask = null;
        }

        public bool Evaluate() => this.IsTriggered;

        private async UniTask ResetTriggerNextFrame(PlayerLoopTiming timing)
        {
            await UniTask.NextFrame(timing);
            this.ResetTrigger();
        }
    }
    #endif
}