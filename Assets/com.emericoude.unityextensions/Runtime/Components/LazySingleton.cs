using UnityEngine;

namespace Emericoude.Framework
{
    /// <summary> A variation of <see cref="Singleton{T}"/> which creates the singleton automatically when first fetched. </summary>
    /// <remarks> This expects to be fully generated at runtime. There should be no existing instance in the scene from the get-go. </remarks>
    public abstract class LazySingleton<T> : MonoBehaviour where T : LazySingleton<T>
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