using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Emericoude.Helpers;
using UnityEngine;
using UnityEngine.VFX;
using ZLinq;

namespace Emericoude.Framework
{
    #if ODIN_INSPECTOR
    [Sirenix.OdinInspector.InfoBox("Holds the object from returning to the pool until all its holder components are ready.")]
    #endif
    public class PooledGameObjectHolderHandler : PooledGameObject
    {
        [Flags]
        public enum CommonPoolHolders
        {
            All = ~0,
            None = 0,
            AudioSource = 1,
            ParticleSystem = 2,
            VisualEffect = 4,
        }
        
        [Tooltip("If true, we will check if the components are ready every 'Tick Rate' in seconds. If false, we will evaluate every single frame.")]
        public bool evaluateUsingTickRate = true;
        [Tooltip("The tick rate used to delay evaluation, if 'Evaluate Using Tick Rate' is set true.")]
        public float tickRate = 0.25f;
        [Tooltip("An initial delay before we start evaluating.")]
        public float startEvaluatingAfter = 0.25f;
        [Tooltip("For each toggled type, adds a holder component, and if so add the proper holders for it. " +
                 "Generally it is better to have this to set 'None', and add the necessary components manually to your prefabs. " +
                 "This is done in awake.")]
        public CommonPoolHolders automaticallyAddHoldersFor = CommonPoolHolders.All;
        
        private PooledGameObjectHolder[] holders;
        private Coroutine waitingForHolderCoroutine;

        private void Awake()
        {
            this.AddCommonHolderComponents();
        }

        private void Start()
        {
            this.UpdateHolderReferences();
        }

        internal override void OnAcquiredFromPool()
        {
            base.OnAcquiredFromPool();
            this.waitingForHolderCoroutine = this.StartCoroutine(this.WaitForHolder());
        }

        internal override void OnReleasedToPool(Transform poolFolder)
        {
            if (this.waitingForHolderCoroutine != null)
            {
                this.StopCoroutine(this.waitingForHolderCoroutine);
                this.waitingForHolderCoroutine = null;
            }
            
            base.OnReleasedToPool(poolFolder);
        }

        /// <summary> Fetches any holder references using GetComponents. Called in start by default. </summary>
        public void UpdateHolderReferences()
        {
            this.holders = this.GetComponents<PooledGameObjectHolder>();
        }

        private IEnumerator WaitForHolder()
        {
            yield return null; //wait one frame in case of initialization
            yield return new WaitForSeconds(this.startEvaluatingAfter);
            while (!this.AreAllHoldersReady())
            {
                if (this.evaluateUsingTickRate) yield return new WaitForSeconds(this.tickRate);
                else yield return new WaitUntil(this.AreAllHoldersReady);
            }
            
            this.ReleaseToPool();
        }

        private bool AreAllHoldersReady()
        {
            return this.holders.Length == 0 || this.holders.AsValueEnumerable().All(h => h.IsReadyForRelease());
        }

        private void AddCommonHolderComponents()
        {
            if (this.automaticallyAddHoldersFor == CommonPoolHolders.None) return;
            
            if (this.automaticallyAddHoldersFor.HasFlag(CommonPoolHolders.AudioSource))
            {
                this.gameObject.GetOrAddComponent<AudioSourcePoolHolder>();
            }

            if (this.automaticallyAddHoldersFor.HasFlag(CommonPoolHolders.ParticleSystem))
            {
                this.gameObject.GetOrAddComponent<ParticleSystemPoolHolder>();
            }

            if (this.automaticallyAddHoldersFor.HasFlag(CommonPoolHolders.VisualEffect))
            {
                this.gameObject.GetOrAddComponent<VisualEffectPoolHolder>();
            }
        }
    }
}