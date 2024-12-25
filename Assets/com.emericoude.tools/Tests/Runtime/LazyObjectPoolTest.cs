using System.Collections;
using Emericoude.Framework;
using UnityEngine;
using UnityEngine.VFX;

/// <summary> A simple singleton object pool for vfx. Feed it <see cref="VisualEffect"/> prefabs and it will group them into individual pools. </summary>
public class LazyObjectPoolTest : LazyObjectPool<LazyObjectPoolTest, string, GameObject>
{
    [SerializeField] private GameObject prefabTemplate;
    
    private const float WAIT_FOR_EFFECT_TICK_RATE = 0.1f;
    private bool currentTargetShouldAutoPlay = false;

    private IEnumerator Start()
    {
        while (true)
        {
            Debug.Log("Requesting an object");
            var getObject = this.GetOrCreateEffect(this.prefabTemplate, true);
            Debug.Log("Retrieved an object");
            yield return new WaitForSeconds(2f);
        }
    }

    /// <summary> Use this instead of GetOrCreate to make sure autoPlay is set for the next get. </summary>
    /// <param name="autoPlayAndAutoRelease"> If true, the effect's <see cref="VisualEffect.Play()"/> will be called,
    /// and a coroutine will check every 0.1s if the effect's <see cref="VisualEffect.HasAnySystemAwake"/> return false.
    /// Once no more systems are awake, the effect will be release. Note that, if set to true, there is no additional
    /// safety net for if you manually release the effect later implemented currently. </param>
    /// <returns> A visual effect from the correct object pool. </returns>
    public GameObject GetOrCreateEffect(GameObject prefab, bool autoPlayAndAutoRelease)
    {
        this.currentTargetShouldAutoPlay = autoPlayAndAutoRelease;
        return this.GetOrCreate(prefab);
    }
    
    /// <returns> The gameobject.name. DO NOT RENAME INSTANCES OF VISUAL EFFECTS. </returns>
    public override string GetObjectKey(GameObject prefab)
    {
        return prefab.gameObject.name;
    }

    protected override GameObject CreatePoolObject()
    {
        Debug.Log("Creating pooled object");
        var visualEffectInstance = base.CreatePoolObject();
        visualEffectInstance.gameObject.name = this.CurrentKey;
        return visualEffectInstance;
    }

    protected override void OnGetPoolObject(GameObject effect)
    {
        Debug.Log("Getting pooled object");
        effect.gameObject.SetActive(true);
        if (this.currentTargetShouldAutoPlay)
        {
            this.StartCoroutine(this.WaitForEffectToBeDone(effect));
        }
    }

    protected override void OnReleasePoolObject(GameObject effect)
    {
        Debug.Log("Releasing pooled object");
        effect.gameObject.SetActive(false);
    }

    protected override void OnDestroyPoolObject(GameObject effect)
    {
        Debug.Log("Destroying pooled object");
        Destroy(effect.gameObject);
    }

    private IEnumerator WaitForEffectToBeDone(GameObject effect)
    {
        yield return new WaitForSeconds(WAIT_FOR_EFFECT_TICK_RATE);
        this.ObjectPools[this.GetObjectKey(effect)].Release(effect);
    }
}
