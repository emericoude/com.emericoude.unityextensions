using UnityEngine;
using UnityEngine.Serialization;

using ZLinq;

namespace Emericoude.Framework
{
    public abstract class PooledComponentsHolder<T> : PooledGameObjectHolder where T : Component
    {
        [SerializeField] protected T[] components;

        protected virtual void Awake() {
            this.components ??= this.GetComponentsInChildren<T>();
        }

        public override bool IsReadyForRelease() {
            if (this.components == null) return true;
            if (this.components.Length == 0) return true;
            return this.components.AsValueEnumerable().All(this.IsComponentReadyForRelease);
        }

        protected abstract bool IsComponentReadyForRelease(T component);
    }
}