using System.Linq;

using UnityEditor;
using UnityEngine;

using Emericoude.Math;

namespace Emericoude.Editor
{
	[CustomPropertyDrawer(typeof(BetterCurveFieldAttribute))]
	public class BetterCurveDrawer : PropertyDrawer
	{
		private BetterCurveFieldAttribute _attribute;
		private BetterCurveFieldAttribute Attribute => _attribute ??= (BetterCurveFieldAttribute)this.attribute;
		
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			GUIContent propertyLabel = EditorGUI.BeginProperty(position, label, property);
			
			Rect foldoutRect = new Rect(position);
			foldoutRect.height = EditorGUIUtility.singleLineHeight;
			
			this.DrawFoldoutHeader(foldoutRect, property, propertyLabel);
			this.Attribute.isFoldout = EditorGUI.Foldout(foldoutRect, this.Attribute.isFoldout, label, toggleOnLabelClick: true);
			if (this.Attribute.isFoldout) this.DrawFoldoutDrawer(position, property, propertyLabel);
			
			EditorGUI.EndProperty();
		}

		public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
		{
			return this.Attribute.isFoldout
				? (EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing) * (property.animationCurveValue.keys.Length + 4)
				: EditorGUIUtility.singleLineHeight;
		}

		private void DrawFoldoutDrawer(Rect position, SerializedProperty property, GUIContent propertyLabel)
		{
			position.height = EditorGUIUtility.singleLineHeight;
			position.y += EditorGUIUtility.singleLineHeight * 3f + EditorGUIUtility.standardVerticalSpacing;
			position.x += position.width * 0.05f;
			position.width -= position.width * 0.05f;
			
			
			var backingCurve = this.CreateBackingCurve(property);
			
			//scale
			Keyframe lastKeyframe = backingCurve.keys.Last();
			var scaleFloatFields = new float[] { lastKeyframe.time, lastKeyframe.value };
			var scaleGuiContents = new GUIContent[]
			{
				new ($"{this.Attribute.TimePropertyLabel} Scale", $"Use this to scale the time of the entire curve. This is generally the best way to modify to overall values while maintaining the exact shape fo the curve."), 
				new ($"{this.Attribute.ValuePropertyLabel} Scale", $"Use this to scale the value of the entire curve. This is generally the best way to modify to overall values while maintaining the exact shape fo the curve.")
			};
			EditorGUI.BeginChangeCheck();
			EditorGUI.MultiFloatField(position, scaleGuiContents, scaleFloatFields);
			if (EditorGUI.EndChangeCheck())
			{
				//apply changes to backing curve
				float xScale = scaleFloatFields[0] / lastKeyframe.time;
				float yScale = scaleFloatFields[1] / lastKeyframe.value;
				backingCurve = backingCurve.Scale(xScale, yScale);
			}
			
			position.x += position.width * 0.05f;
			position.width -= position.width * 0.05f;
			
			//keys
			for (int i = 0; i < property.animationCurveValue.length; i++)
			{
				Keyframe keyframe = property.animationCurveValue.keys[i];
				var floatFields = new float[] { keyframe.time, keyframe.value };
				var guiContents = new GUIContent[]
				{
					new ($"Key {i}:   {this.Attribute.TimePropertyLabel}", $"Use this to move the key's time (x-axis), keeping relatively accurate tangents."), 
					new ($"  {this.Attribute.ValuePropertyLabel}", $"Use this to move the key's value (y-axis), keeping relatively accurate tangents.")
				};
				
				position.position += new Vector2(0.0f, EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing);
				position.height = EditorGUIUtility.singleLineHeight;
				
				EditorGUI.BeginChangeCheck();
				EditorGUI.MultiFloatField(
					position, 
					guiContents, 
					floatFields
				);
				if (EditorGUI.EndChangeCheck())
				{
					backingCurve.MoveKey(i, floatFields[0], floatFields[1]);
				}
			}

			property.animationCurveValue = backingCurve;
		}

		private void DrawFoldoutHeader(Rect position, SerializedProperty property, GUIContent propertyLabel)
		{
			if (!Attribute.isFoldout)
			{
				position.x += EditorGUIUtility.labelWidth;
				position.width -= EditorGUIUtility.labelWidth;
			}
			else
			{
				position.height += EditorGUIUtility.singleLineHeight * 2f;
			}

			property.animationCurveValue = EditorGUI.CurveField(position, GUIContent.none, property.animationCurveValue);
		}

		private AnimationCurve CreateBackingCurve(SerializedProperty property)
		{
			//Create backing curve, this is to prevent EditorGUI.CurveField to override the manipulations we do here
			AnimationCurve backingCurve = new AnimationCurve();
			backingCurve.CopyFrom(property.animationCurveValue);

			if (backingCurve.keys.Length <= 1)
			{
				backingCurve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
			}

			return backingCurve;
		}
	}
}
