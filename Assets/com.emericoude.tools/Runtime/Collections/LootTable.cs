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
            float totalDropChance = this.loot.Select(item => item.dropChance).Sum();
            float randomDropValue = Random.Range(0f, totalDropChance);
            float dropValue = 0f;
            foreach (var droppable in this.loot)
            {
                dropValue += droppable.dropChance;
                if (randomDropValue <= dropValue)
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
            foreach (var droppable in this.loot)
            {
                if (Random.Range(0f, 100f) <= droppable.dropChance)
                {
                    itemListDrop.Add(droppable.item);
                }
            }

            return itemListDrop;
        }
        
        #region Enumerator
        
        public int Count => this.loot.Count;
        
        public IEnumerator<T> GetEnumerator()
        {
            return this.loot.Select(droppable => droppable.item).GetEnumerator();
        }
        
        IEnumerator IEnumerable.GetEnumerator () {
            return this.GetEnumerator();
        }
        
        public T this[int index] {
            get {
                if (index >= 0 && index < this.loot.Count) {
                    return this.loot[index].item;
                }
                throw new IndexOutOfRangeException("Index is out of range for LootTable.");
            }
            set {
                if (index >= 0 && index < this.loot.Count) {
                    this.loot[index] = new Loot { item = value, dropChance = 0f };
                } else {
                    throw new IndexOutOfRangeException("Index is out of range for LootTable.");
                }
            }
        }

        public void Add(T item, float dropChance)
        {
            this.loot.Add(new Loot() { item = item, dropChance = dropChance });
        }
        
        public void RemoveAt(int index) {
            this.loot.RemoveAt(index);
        }
        
        #endregion
    }
}
