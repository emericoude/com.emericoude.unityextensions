using System;
using UnityEngine;

namespace Emericoude
{
    /// <summary> Add this on angle fields where that you want to display as degrees in the inspector, but store as radians. </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public class RadiansDisplayedAsDegreesAttribute : PropertyAttribute
    {
        
    }
}
