using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

using Random = UnityEngine.Random;

namespace Emericoude.Collections
{
    [Serializable, HideLabel]
    public class LootTable<T> : IEnumerable<T>
    {
        [Serializable]
        public struct Loot
        {
            #if ODIN_INSPECTOR
            [HorizontalGroup("loot-header", width: 0.85f)]
            public T item;
            [HorizontalGroup("loot-header", width: 0.15f), HideLabel, SuffixLabel("%", true)] 
            public float dropChance;
            #else 
            public float dropChance;
            public T item;
            #endif
        }
        
        public List<Loot> loot = new List<Loot>();
        
        /// <summary> Generates a random item drop. </summary>
        /// <returns> A random item from the loot table based on weighted drop chance. </returns>
        public T GetRandomItemDrop()
        {
            var totalDropChance = loot.Select(item => item.dropChance).Sum();
            var randomDropValue = Random.Range(0f, totalDropChance);
            var dropValue = 0f;
            foreach (var droppable in loot)
            {
                dropValue += droppable.dropChance;
                if (randomDropValue < dropValue)
                {
                    return droppable.item;
                }
            }

            return default;
        }
        
        /// <summary>
        /// As opposed to <see cref="GetRandomItemDrop"/>, this evaluates each item individually one by one,
        /// and creates a drop list from the loot table.
        /// </summary>
        /// <returns> A list of random items, weighted based on drop chance. </returns>
        public List<T> GetRandomItemListDrop()
        {
            var itemListDrop = new List<T>();
            var totalDropChance = loot.Select(item => item.dropChance).Sum();
            foreach (var droppable in loot)
            {
                var randomDropChanceValue = Random.Range(0f, totalDropChance);
                if (droppable.dropChance < randomDropChanceValue)
                {
                    itemListDrop.Add(droppable.item);
                }
            }

            return itemListDrop;
        }
        
        #region Enumerator
        
        public int Count => loot.Count;
        
        public IEnumerator<T> GetEnumerator()
        {
            return loot.Select(droppable => droppable.item).GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator () {
            return GetEnumerator();
        }
        
        public T this[int index] {
            get {
                if (index >= 0 && index < loot.Count) {
                    return loot[index].item;
                }
                throw new IndexOutOfRangeException("Index is out of range for LootTable.");
            }
            set {
                if (index >= 0 && index < loot.Count) {
                    loot[index] = new Loot { item = value, dropChance = 0f };
                } else {
                    throw new IndexOutOfRangeException("Index is out of range for LootTable.");
                }
            }
        }

        public void Add(T item, float dropChance)
        {
            loot.Add(new Loot() { item = item, dropChance = dropChance });
        }
        
        public void RemoveAt(int index) {
            loot.RemoveAt(index);
        }
        
        #endregion
    }
}
