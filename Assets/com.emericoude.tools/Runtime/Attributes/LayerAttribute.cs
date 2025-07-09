using System;
using UnityEngine;

namespace Emericoude
{
    /// <summary> Attribute to select a single layer. Apply to an int field. </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class LayerAttribute : PropertyAttribute {
        
    }
}