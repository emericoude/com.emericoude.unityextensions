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
    /// <remarks> Use <see cref="ClearPool(TObject)"/> once in a while when you no longer need a specific group. </remarks>
    public abstract class LazyObjectPool<TSingleton, TKey, TObject> : LazySingleton<TSingleton> 
        where TSingleton : LazySingleton<TSingleton>
        where TObject : Object 
    {
        /// <summary> The object pool dictionary. </summary>
        protected readonly Dictionary<TKey, ObjectPool<TObject>> ObjectPools = new();
        
        /// <summary> The template dictionary. </summary>
        protected readonly Dictionary<TKey, TObject> Templates = new();
        
        /// <summary> The current key being fetched. This is used as Unity's CreatePoolObject doesn't pass any arguments.
        /// We can instead use this as the argument whenever we do a .Get(), but instead require to create a new object in the pool. </summary>
        protected TKey CurrentKey;

        /// <summary> Disposes of all the pools and their objects. Effectively empties out this component. </summary>
        public void ClearPools()
        {
            foreach (var pool in this.ObjectPools.Values)
            {
                pool.Dispose();
            }
            
            this.ObjectPools.Clear();
            this.Templates.Clear();
            this.CurrentKey = default;
        }

        /// <summary> Clears the pool associated with the specified template.
        /// Use this to save on memory once you know you won't need this pool anymore. </summary>
        public void ClearPool(TObject template) => this.ClearPool(this.GetObjectKey(template));
        
        /// <summary> Clears the pool associated with the specified key.
        /// Use this to save on memory once you know you won't need this pool anymore. </summary>
        public void ClearPool(TKey key)
        {
            if (this.ObjectPools.TryGetValue(key, out var pool))
            {
                pool.Dispose();
            }
            
            this.ObjectPools.Remove(key);
            this.Templates.Remove(key);
        }
        
        /// <returns> If the key is valid, a pooled or new object from this list; otherwise null. </returns>
        /// <remarks> See <see cref="GetObjectKey(TObject)"/> for more info on how to get keys. </remarks>
        public TObject GetOrCreate(TKey key)
        {
            if (!this.ObjectPools.ContainsKey(key))
            {
                Debug.LogError("No such key in pool.");
                return null;
            }

            this.CurrentKey = key;
            return this.ObjectPools[key].Get();
        }

        /// <returns> Gets an object from the pool, if the pool for this template doesn't exist, creates one. </returns>
        public TObject GetOrCreate(TObject template)
        {
            var key = this.GetObjectKey(template);
            if (!this.ObjectPools.TryGetValue(key, out var pool))
            {
                pool = this.CreatePool();
                this.ObjectPools.Add(key, pool);
                this.Templates.Add(key, template);
            }

            this.CurrentKey = key;
            return pool.Get();
        }

        /// <summary> Releases an object from its pool. </summary>
        public void Release(TObject objectInstance) => this.ObjectPools[this.GetObjectKey(objectInstance)].Release(objectInstance);
        
        /// <returns> A key used for grouping pools of similar things. </returns>
        /// <remarks> This should ideally return the same key for a template or an instance of it. </remarks>
        public abstract TKey GetObjectKey(TObject template);
        
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
        protected virtual TObject CreatePoolObject()
        {
            return Instantiate(this.Templates[this.CurrentKey], this.transform);
        }
        
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
        protected virtual void OnDestroyPoolObject(TObject poolObject)
        {
            Destroy(poolObject);
        }
    }
}
