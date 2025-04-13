using System;
using UnityEngine;

namespace Emericoude.Framework
{
    
    /// <summary> A component to interface with <see cref="GenericGameObjectPoolManager"/>. </summary>
    public class PooledGameObject : MonoBehaviour
    {
        public int Key { get; internal set; }
        public Component Component { get; internal set; }
        public Type ComponentType => this.Component.GetType();

        internal virtual void OnAcquiredFromPool()
        {
            this.gameObject.SetActive(true);
        }

        internal virtual void OnReleasedToPool(Transform poolFolder)
        {
            this.gameObject.SetActive(false);
            this.transform.SetParent(poolFolder);
        }

        public void ReleaseToPool()
        {
            GenericGameObjectPoolManager.Instance.Release(this);
        }
    }
}
