using System.Collections;
using Emericoude.Framework;
using UnityEngine;

namespace Emericoude.Tests
{
    public class PoolingTests : MonoBehaviour
    {
        public ParticleSystem pooledParticleSystemPrefab;

        private IEnumerator Start()
        {
            while (true)
            {
                var instance = GenericGameObjectPoolManager.Instance.GetOrCreateFromPrefab(this.pooledParticleSystemPrefab);
                instance.transform.position = Random.insideUnitSphere;
                yield return new WaitForSeconds(0.1f);
            }
        }
    }
}
