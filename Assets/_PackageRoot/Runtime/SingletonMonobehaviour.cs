using System.Collections;
using System.Collections.Generic;

using UnityEngine;

namespace Emeric.Utilities
{
    /// <summary> A base implementation of the singleton pattern. </summary>
    /// <remarks> 
    /// If you want persistence, use <see cref="PersistentSingletonMonobehaviour{T}"/> instead. <br/>
    /// If you want the singleton to be generated if it is missing, create your own implementation instead.
    /// </remarks>
    public abstract class SingletonMonobehaviour<T> : MonoBehaviour where T : SingletonMonobehaviour<T>
    {
        protected static T _instance;
        public static T Instance { get { return _instance; } }

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

    /// <summary> A variation of <see cref="SingletonMonobehaviour{T}"/> which adds persistence (<see cref="Object.DontDestroyOnLoad(Object)"/>). </summary>
    public abstract class PersistentSingletonMonobehaviour<T> : SingletonMonobehaviour<T> where T : PersistentSingletonMonobehaviour<T>
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
}
