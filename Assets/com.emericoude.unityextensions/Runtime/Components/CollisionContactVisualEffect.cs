using System.Linq;
using Emericoude.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.VFX;

namespace Emericoude
{
    public class CollisionContactVisualEffect : CollisionContactEventListener
    {
        [Header("VFX Settings")]
        #if ODIN_INSPECTOR
        [AssetsOnly]
        #endif
        [SerializeField] private VisualEffect visualEffectPrefab;
        
        protected override void OnCollisionContact(Collision collision)
        {
            var effectInstance = LazyVisualEffectPool.Instance.GetOrCreateEffect(this.visualEffectPrefab, true);
            effectInstance.transform.position = collision.contacts.Last().point;
        }
    }
}
