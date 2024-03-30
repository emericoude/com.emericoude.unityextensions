using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Emericoude.UnityExtensions
{
    /// <summary> A base implementation of the singleton pattern. </summary>
    /// <remarks> If you want persistence, use <see cref="PersistentSingletonMonoBehaviour{T}"/> instead. <br/>
    /// If you want a "lazy" singleton (i.e. to be created when first fetched), use <see cref="LazySingletonMonoBehaviour{T}{T}"/> instead. </remarks>
    [DefaultExecutionOrder(-10)]
    public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : SingletonMonoBehaviour<T>
    {
        /// <summary> The instance of this singleton. </summary>
        public static T Instance { get { return _instance; } }
        protected static T _instance;

        protected virtual void Awake()
        {
            SingletonInitialization();
        }

        /// <summary> If no other instance exists, assign the singleton <see href="Instance"/> as <see langword="this"/>. </summary>
        /// <remarks> If another instance exists, this instance will destroy itself. </remarks>
        /// <returns> True if this is the new  <see href="Instance"/>; otherwise false. </returns>
        protected virtual bool SingletonInitialization()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
                return false;
            }

            _instance = (T)this;
            return true;
        }
    }

    /// <summary> A variation of <see cref="SingletonMonoBehaviour{T}"/> which adds persistence (<see cref="Object.DontDestroyOnLoad(Object)"/>). </summary>
    [DefaultExecutionOrder(-10)]
    public abstract class PersistentSingletonMonoBehaviour<T> : SingletonMonoBehaviour<T> where T : PersistentSingletonMonoBehaviour<T>
    {
        protected override bool SingletonInitialization()
        {
            if (base.SingletonInitialization())
            {
                DontDestroyOnLoad(this);
                return true;
            }

            return false;
        }
    }

    /// <summary> A variation of <see cref="SingletonMonoBehaviour{T}"/> which creates the singleton automatically when first fetched. </summary>
    /// <remarks> This expects to be fully generated at runtime. There should be no existing instance in the scene from the get-go. </remarks>
    public abstract class LazySingletonMonoBehaviour<T> : MonoBehaviour where T : LazySingletonMonoBehaviour<T>
    {
        private static T _instance;

        /// <summary> The instance of this singleton. </summary>
        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new GameObject(typeof(T).Name).AddComponent<T>();
                    _instance.SingletonAwake();
                }
                return _instance;
            }
        }

        /// <summary> Called when the singleton is created as it is first fetched. </summary>
        /// <remarks> Override this to add any initialization you might need before receiving the instance. </remarks>
        protected virtual void SingletonAwake() { }
    }
}
