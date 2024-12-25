using UnityEngine;

namespace Emericoude.Framework
{
    /// <summary> A variation of <see cref="Singleton{T}"/> which adds persistence (<see cref="Object.DontDestroyOnLoad(Object)"/>). </summary>
    [DefaultExecutionOrder(-10)]
    public abstract class PersistentSingleton<T> : Singleton<T> where T : PersistentSingleton<T>
    {
        protected override bool SingletonInitialization()
        {
            if (!base.SingletonInitialization()) return false;
            
            DontDestroyOnLoad(this);
            return true;
        }
    }
}