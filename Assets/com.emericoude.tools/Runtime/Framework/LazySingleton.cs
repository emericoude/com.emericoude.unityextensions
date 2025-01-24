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

        /// <summary> Retrieve the instance with extra safety checks (such as to avoid leaks when unloading a scene). </summary>
        /// <param name="caller"> Whichever gameobject is trying to access the singleton. </param>
        /// <remarks> It is recommended to use this when trying to access Instance from OnDisable or OnDestroy mostly, you can also check for gameObject.scene.isLoaded manually. </remarks>
        /// <returns> If the scene of the caller is loaded, <see cref="Instance"/>; otherwise, null. </returns>
        public static T GetInstance(GameObject caller)
        {
            return caller.scene.isLoaded ? Instance : null;
        }

        /// <summary> Called when the singleton is created as it is first fetched. </summary>
        /// <remarks> Override this to add any initialization you might need before receiving the instance. </remarks>
        protected virtual void SingletonAwake() { }
    }
}