using System;
using System.Collections.Generic;
using System.Linq;
using Emericoude.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

namespace Emericoude.Tests
{
    public class LootTableTests : MonoBehaviour
    {
        [Serializable, HideLabel]
        public struct TestStruct
        {
            public string name;
        }
    
        public LootTable<TestStruct> lootTableTest = new LootTable<TestStruct>();
        public int iterations = 10000;
        
        private Dictionary<string, int> values = new Dictionary<string,int>();

        [Button("Print Random Item")]
        public void PrintRandomItem()
        {
            Debug.Log($"Drop: {this.lootTableTest.GetRandomItemDrop().name}");
        }

        [Button("Print Random Item List")]
        public void PrintRandomItemList()
        {
            Debug.Log($"Drops: {string.Join(", ", this.lootTableTest.GetRandomItemListDrop().Select(_ => _.name))}"); 
        }

        [Button("Do iteration test")]
        public void PickRandomItemByIterations()
        {
            this.values = new Dictionary<string, int>();
            foreach (var item in this.lootTableTest)
            {
                this.values.Add(item.name, 0);
            }
            
            for (int i = 0; i < this.iterations; i++)
            {
                string value = this.lootTableTest.GetRandomItemDrop().name;
                this.values[value]++;
            }
            
            Debug.Log($"{string.Join(", ", this.values.Select(v => $"{v.Key}: {v.Value} ({(v.Value/(float)this.iterations)*100f})"))}");
        }
    }
}
