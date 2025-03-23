using System;
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
            [TextArea]
            public string text;
            [Range(0, 1f)]
            public float number;
        }
    
        public LootTable<TestStruct> lootTableTest = new LootTable<TestStruct>();

        [Button("Print Random Item")]
        public void PrintRandomItem()
        {
            Debug.Log($"Drop: {this.lootTableTest.GetRandomItemDrop().text}");
        }

        [Button("Print Random Item List")]
        public void PrintRandomItemList()
        {
            Debug.Log($"Drops: {string.Join(", ", this.lootTableTest.GetRandomItemListDrop().Select(_ => _.text))}"); 
        }
    }
}
