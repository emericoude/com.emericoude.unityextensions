using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace Emericoude.Framework
{
    /// <summary>
    /// A generic gameobject pooling manager. Pools are organized by component Type, and then by prefab InstanceID
    /// This is mainly a way to get pooling for most common things (AudioSource, ParticleSystems, etc.) quickly.
    /// You may want more precise pooling implementations. Oh, also, this is a lazy singleton.
    /// This is not multithreading friendly.
    /// <para/> General Guide:
    /// <br/> - You can either call GenericGameObjectPoolManager.Instance.GetOrCreateFromPrefab, or use the static calls GenericGameObjectPoolManager.InstantiatePrefabIntoPool
    /// <br/> - To release an object to the pool, consider adding PooledGameObject component(s) to your object.
    /// For examples, <see cref="TimedPooledGameObject"/> and <see cref="PooledGameObjectHolderHandler"/>.
    /// Otherwise, you can call Release (either through GenericGameObjectPoolManager.Instance or the PooledGameObject component).
    /// </summary>
    /// <remarks> For more custom solutions, consider using the <see cref="LazyObjectPool{TSingleton,TKey,TObject}"/> base. </remarks>
    #if ODIN_INSPECTOR
    [Sirenix.OdinInspector.InfoBox("Generic object pool manager.")]
    #endif
    public class GenericGameObjectPoolManager : LazySingleton<GenericGameObjectPoolManager>
    {
        private struct ComponentPoolEntry
        {
            public readonly ObjectPool<PooledGameObject> Pool;
            public readonly Component Prefab;
            public readonly Transform Folder;

            public ComponentPoolEntry(ObjectPool<PooledGameObject> pool, Component prefab, Transform folder)
            {
                this.Pool = pool;
                this.Prefab = prefab;
                this.Folder = folder;
            }
        }
        
        private readonly Dictionary<Type, Dictionary<int, ComponentPoolEntry>> pools = new();
        
        private Type currentType;
        private int currentKey;

        /// <summary> Instantiates the prefab into an object pool to be recycled later. </summary>
        /// <remarks> In this specific use-case, the instances will be stored in the Transform type dictionary. </remarks>
        /// <returns> The new or recycled instance. </returns>
        public static GameObject InstantiatePrefabIntoPool(GameObject prefab)
        {
            return GenericGameObjectPoolManager.Instance.GetOrCreateFromPrefab(prefab.transform).gameObject;
        }

        /// <summary> Instantiates the prefab into an object pool to be recycled later. </summary>
        /// <returns> The new or recycled instance. </returns>
        public static T InstantiatePrefabIntoPool<T>(T prefab) where T : Component
        {
            return GenericGameObjectPoolManager.Instance.GetOrCreateFromPrefab(prefab);
        }
        
        /// <summary> Gets an object from the pool, using information from the provided prefab. </summary>
        /// <param name="prefab"> Must be a prefab asset. If it is not, shit will break. </param>
        /// <typeparam name="T"> Must be of type component. You can pass in the transform if you don't want any specific component. </typeparam>
        /// <returns> A new or recycled object from an object pool. </returns>
        public T GetOrCreateFromPrefab<T>(T prefab) where T : Component
        {
            this.currentType = prefab.GetType();
            this.currentKey = prefab.gameObject.GetInstanceID();
            
            return this.GetOrCreateObjectPool(prefab).Get().Component as T;
        }
        
        /// <summary> Return the instance to the pool. </summary>
        public void Release(PooledGameObject instance)
        {
            this.pools[instance.ComponentType][instance.Key].Pool.Release(instance);
        }

        private ObjectPool<PooledGameObject> GetOrCreateObjectPool(Component prefab)
        {
            if (!this.pools.ContainsKey(this.currentType))
            {
                this.pools.Add(this.currentType, new Dictionary<int, ComponentPoolEntry>());
            }

            if (!this.pools[this.currentType].ContainsKey(this.currentKey))
            {
                var folder = new GameObject($"Pool ({this.currentType}) - {prefab.name}").transform;
                folder.SetParent(this.transform);
                
                this.pools[this.currentType].Add(this.currentKey, new ComponentPoolEntry(this.CreatePool(), prefab, folder));
            }

            return this.pools[this.currentType][this.currentKey].Pool;
        }

        private ObjectPool<PooledGameObject> CreatePool()
        {
            return new ObjectPool<PooledGameObject>(
                this.CreatePoolObject,
                this.OnGetPoolObject,
                this.OnReleasePoolObject,
                this.OnDestroyPoolObject
            );
        }
        
        private PooledGameObject CreatePoolObject()
        {
            var poolEntry = this.pools[this.currentType][this.currentKey];
            var componentInstance = Instantiate(poolEntry.Prefab, poolEntry.Folder);
            if (!componentInstance.TryGetComponent(out PooledGameObject pooledGameObject))
            {
                pooledGameObject = componentInstance.gameObject.AddComponent<PooledGameObject>();
            }
            
            pooledGameObject.Key = this.currentKey;
            pooledGameObject.Component = componentInstance;
            return pooledGameObject;
        }
        
        private void OnGetPoolObject(PooledGameObject poolObject)
        {
            poolObject.OnAcquiredFromPool();
        }
        
        protected void OnReleasePoolObject(PooledGameObject poolObject)
        {
            poolObject.OnReleasedToPool(this.pools[poolObject.ComponentType][poolObject.Key].Folder);
        }
        
        protected void OnDestroyPoolObject(PooledGameObject poolObject)
        {
            Destroy(poolObject.gameObject);
        }
    }
}