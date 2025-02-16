using System.Collections;
using Emericoude.Attributes;
using UnityEngine;

namespace Emericoude.Tests
{
    [RequireComponent(typeof(BoxCollider))]
    public class TimeManagerTests : MonoBehaviour
    {
        [Header("Spawns")]
        [SerializeField] private GameObject spawnPrefab;
        [SerializeField] private float spawnRate = 0.05f;
        [SerializeField] private BoxCollider spawnArea;
        
        [Header("Curves")]
        [SerializeField] private TimeEffect timeEffectA;
        [SerializeField] private TimeEffect timeEffectB;
        [SerializeField] private TimeEffect timeEffectC;
        [BetterCurveField("Timer", "Time Scale Value")]
        [SerializeField] private AnimationCurve stopCurve;

        private void Awake()
        {
            this.spawnArea = this.GetComponent<BoxCollider>();
        }

        private IEnumerator Start()
        {
            while (true) {
                Vector3 spawnPosition = new Vector3(
                    Random.Range(this.spawnArea.bounds.min.x, this.spawnArea.bounds.max.x),
                    Random.Range(this.spawnArea.bounds.min.y, this.spawnArea.bounds.max.y),
                    Random.Range(this.spawnArea.bounds.min.z, this.spawnArea.bounds.max.z)
                );
                Instantiate(this.spawnPrefab, spawnPosition, Random.rotation);
                yield return new WaitForSeconds(this.spawnRate);
            }
        }

        public void TriggerEffectA()
        {
            TimeManager.Instance.StartTimeEffectWithUniqueName(this.timeEffectA, "A", true);
        }
        
        public void TriggerEffectB()
        {
            TimeManager.Instance.StartTimeEffectWithUniqueName(this.timeEffectB, "B", true);
        }
        
        public void TriggerEffectC()
        {
            TimeManager.Instance.StartTimeEffectWithUniqueName(this.timeEffectC, "C", true);
        }
        
        public void StopEffectA()
        {
            TimeManager.Instance.StopTimeEffectWithName("A", this.stopCurve);
        }
        
        public void StopEffectB()
        {
            TimeManager.Instance.StopTimeEffectWithName("B", this.stopCurve);
        }
        
        public void StopEffectC()
        {
            TimeManager.Instance.StopTimeEffectWithName("C", this.stopCurve);
        }
    }
}