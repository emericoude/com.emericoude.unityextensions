using System;
using Emericoude.StateMachine;
using UnityEngine;

namespace Emericoude.Tests
{
    public class TriggerPredicateTests : MonoBehaviour
    {
#if CYSHARP_UNITTASK
        [SerializeField] private float triggerDelay = 2f;
        private float timer;
        
        private SingleUseTriggerPredicate singleUseTriggerPredicate = new();
        
        private SingleFrameTriggerPredicate singleFrameTriggerPredicate = new();

        private void Awake()
        {
            timer = triggerDelay;
        }

        private void Update()
        {
            timer -= Time.deltaTime;
            if (timer <= 0f)
            {
                timer = triggerDelay;
                
                singleFrameTriggerPredicate.Trigger();
                singleUseTriggerPredicate.Trigger();
                Debug.Log("Triggered!");
            }
            
            Debug.Log($"Evaluate 1: frame: {singleFrameTriggerPredicate.Evaluate()}, single: {singleUseTriggerPredicate.Evaluate()}");
            Debug.Log($"Evaluate 2: frame: {singleFrameTriggerPredicate.Evaluate()}, single: {singleUseTriggerPredicate.Evaluate()}");
        }
#endif
    }
}