using Emericoude.Helpers;

namespace Emericoude.Framework
{
    public class TimedPooledGameObject : PooledGameObject
    {
        public float lifeDuration = 5f;
        public DeltaTimeScale timeScale = DeltaTimeScale.DeltaTime;
        private float timer;
        
        protected virtual void OnEnable()
        {
            this.timer = this.lifeDuration;
        }

        private void Update()
        {
            this.timer -= this.timeScale.GetDeltaTime();
            if (this.timer <= 0f)
            {
                this.ReleaseToPool();
            }
        }
    }
}