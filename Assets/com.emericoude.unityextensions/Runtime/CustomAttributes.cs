using System;
using UnityEngine;

namespace Emericoude
{
	/// <summary> Draws a curve's duration and value fields to be modified easily and while keeping the curve's shape, 
	/// without needing to go into the curve editor. </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class BetterCurveFieldAttribute : PropertyAttribute { }
}
