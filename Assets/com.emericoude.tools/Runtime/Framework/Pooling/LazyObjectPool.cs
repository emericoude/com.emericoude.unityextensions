using System.Collections.Generic;
using Emericoude.Framework;
using UnityEngine;
using UnityEngine.Pool;

namespace Emericoude.Framework
{
    /// <summary> A lazy singleton that contains a dictionary of pools. A good use-case of this is a VisualEffectsPool,
    /// where you have different VFX prefabs which are reused, but you still want to pool them. </summary>
    /// <typeparam name="TSingleton"> The type of your singleton. </typeparam>
    /// <typeparam name="TKey"> The type to be used for keys. <see cref="GetObjectKey"/> should retrieve the same key for a template and an instance. </typeparam>
    /// <typeparam name="TObject"> The type of object you want to pool. </typeparam>
    /// <remarks> Use <see cref="FlushPool"/> once in a while when you no longer need a specific group. </remarks>
    public abstract class LazyObjectPool<TSingleton, TKey, TObject> : LazySingleton<TSingleton> 
        where TSingleton : LazySingleton<TSingleton>
        where TObject : Object 
    {
        /// <summary> The object pool dictionary. </summary>
        protected readonly Dictionary<TKey, ObjectPool<TObject>> Pools = new();
        
        /// <summary> The template dictionary. </summary>
        protected readonly Dictionary<TKey, TObject> Templates = new();
        
        /// <summary> The current key being fetched. This is used as Unity's CreatePoolObject doesn't pass any arguments.
        /// We can instead use this as the argument whenever we do a .Get(), but instead require to create a new object in the pool. </summary>
        protected TKey CurrentKey;

        /// <returns> Gets an object from the pool, if the pool for this template doesn't exist, creates one. </returns>
        public TObject GetOrCreateObjectFromPool(TObject template)
        {
            this.CurrentKey = this.GetObjectKey(template);
            return this.GetOrCreatePool(template).Get();
        }

        protected virtual ObjectPool<TObject> GetOrCreatePool(TObject template)
        {
            if (this.Pools.TryGetValue(this.CurrentKey, out var pool)) return pool;
            
            this.Pools.Add(this.CurrentKey, this.CreatePool());
            this.Templates.Add(this.CurrentKey, template);

            return this.Pools[this.CurrentKey];
        }

        /// <summary> Releases an object from its pool. </summary>
        public void Release(TObject objectInstance) => this.Pools[this.GetObjectKey(objectInstance)].Release(objectInstance);
        
        /// <returns> A key used for grouping pools of similar things. </returns>
        /// <remarks> This should ideally return the same key for a template or an instance of it. </remarks>
        public abstract TKey GetObjectKey(TObject template);
        
        /// <summary> Destroys itself. Since its lazy, it will be re-instantiated when needed. </summary>
        public virtual void FlushAllPools()
        {
            Destroy(this.gameObject);
        }

        /// <summary> Clears the pool associated with the specified template.
        /// Use this to save on memory once you know you won't need this pool anymore. </summary>
        public virtual void FlushPool(TObject template)
        {
            this.FlushPool(this.GetObjectKey(template));
        }
        
        /// <summary> Clears the pool associated with the specified key.
        /// Use this to save on memory once you know you won't need this pool anymore. </summary>
        public virtual void FlushPool(TKey key)
        {
            if (this.Pools.TryGetValue(key, out var pool))
            {
                pool.Dispose();
            }
            
            this.Pools.Remove(key);
            this.Templates.Remove(key);
        }
        
        /// <returns> A new object pool. </returns>
        /// <seealso cref="CreatePoolObject"/>
        /// <seealso cref="OnGetPoolObject"/>
        /// <seealso cref="OnReleasePoolObject"/>
        /// <seealso cref="OnDestroyPoolObject"/>
        protected virtual ObjectPool<TObject> CreatePool()
        {
            return new ObjectPool<TObject>(
                this.CreatePoolObject,
                this.OnGetPoolObject,
                this.OnReleasePoolObject,
                this.OnDestroyPoolObject
            );
        }

        /// <summary> Object Pool's CreateFunc. This is used to construct an object pool. By default, this will instantiate the object as a child of this' transform. </summary>
        /// <returns> A new, ready-to-use pool object. </returns>
        /// <seealso cref="OnGetPoolObject"/>
        /// <seealso cref="OnReleasePoolObject"/>
        /// <seealso cref="OnDestroyPoolObject"/>
        protected abstract TObject CreatePoolObject();
        
        /// <returns> Object Pool's ActionOnGet. This is used to construct an object pool. </returns>
        /// <remarks> Use this to make sure the object is ready-to-use. </remarks>
        /// <seealso cref="CreatePoolObject"/>
        /// <seealso cref="OnReleasePoolObject"/>
        /// <seealso cref="OnDestroyPoolObject"/>
        protected abstract void OnGetPoolObject(TObject poolObject);
        
        /// <returns> Object Pool's ActionOnRelease. This is used to construct an object pool. </returns>
        /// <seealso cref="CreatePoolObject"/>
        /// <seealso cref="OnGetPoolObject"/>
        /// <seealso cref="OnDestroyPoolObject"/>
        protected abstract void OnReleasePoolObject(TObject poolObject);

        /// <returns> Object Pool's ActionOnDestroy. This is used to construct an object pool. By default, this will destroy the object. </returns>
        /// <remarks> Use this to make sure the object gets cleaned up properly. </remarks>
        /// <seealso cref="CreatePoolObject"/>
        /// <seealso cref="OnGetPoolObject"/>
        /// <seealso cref="OnReleasePoolObject"/>
        protected abstract void OnDestroyPoolObject(TObject poolObject);
    }
}
