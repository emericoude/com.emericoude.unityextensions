using System;
using UnityEngine;

#if ODIN_INSPECTOR
using Sirenix.OdinInspector;
#endif

namespace Emericoude
{
	/// <summary> Draws a curve's duration and value fields to be modified easily and while keeping the curve's shape, 
	/// without needing to go into the curve editor. </summary>
	[AttributeUsage(AttributeTargets.Field | AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
	public class BetterCurveFieldAttribute : PropertyAttribute
	{
		public readonly string TimePropertyLabel;
		public readonly string ValuePropertyLabel;
		public bool isFoldout;

		public BetterCurveFieldAttribute(string timePropertyLabel = "Time", string valuePropertyLabel = "Value")
		{
			this.TimePropertyLabel = timePropertyLabel;
			this.ValuePropertyLabel = valuePropertyLabel;
		}
	}
	
	/// <summary> Displays the field or property inside an "Info" box. Does nothing if you do not have Odin Inspector. </summary>
	/// <remarks> This combines the following attributes from Odin: [ShowInInspector], [ReadOnly], [VerticalGroup] and [FoldoutGroup]. </remarks>
	#if ODIN_INSPECTOR
	[IncludeMyAttributes]
	[ShowInInspector, ReadOnly, VerticalGroup("Info Parent", PaddingTop = 8f, PaddingBottom = 8f, Order = 999), FoldoutGroup("Info Parent/Debug Info")]
	#endif
	public class DrawInDebugInfoBox : Attribute { }
	
	/// <summary> Removes label and foldout from non-MonoBehaviour. Does nothing if you do not have Odin Inspector. </summary>
	/// <remarks> Combines the following attributes from Odin: [HideLabel], [InLineProperty]. </remarks>
	#if ODIN_INSPECTOR
	[IncludeMyAttributes]
	[HideLabel, InlineProperty]
	#endif
	public class DrawAsPropertyOnly : Attribute { }
}
