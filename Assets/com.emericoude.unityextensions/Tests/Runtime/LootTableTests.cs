using System;
using Emericoude.Collections;
using Sirenix.OdinInspector;
using UnityEngine;

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
}
