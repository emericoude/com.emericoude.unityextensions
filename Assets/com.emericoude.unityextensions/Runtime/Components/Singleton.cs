using UnityEngine;

namespace Emericoude.Framework
{
    /// <summary> A base implementation of the singleton pattern. </summary>
    /// <remarks> If you want persistence, use <see cref="PersistentSingleton{T}"/> instead. <br/>
    /// If you want a "lazy" singleton (i.e. to be created when first fetched), use <see cref="LazySingleton{T}"/> instead. </remarks>
    [DefaultExecutionOrder(-10)]
    public abstract class Singleton<T> : MonoBehaviour where T : Singleton<T>
    {
        /// <summary> The instance of this singleton. </summary>
        public static T Instance => _instance;
        private static T _instance;

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
}
