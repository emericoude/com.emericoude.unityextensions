using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;
using UnityEngine;

using Emericoude.Math;

namespace Emericoude.Editor
{
	[CustomPropertyDrawer(typeof(BetterCurveFieldAttribute))]
	public class BetterCurveDrawer : PropertyDrawer
	{
		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			GUIContent propertyLabel = EditorGUI.BeginProperty(position, label, property);

			//Record the last keyframe before changes are made
			Keyframe lastKeyframe = property.animationCurveValue.keys.LastOrDefault();
			float[] floatFields = new float[2] { lastKeyframe.time, lastKeyframe.value };

			EditorGUI.BeginChangeCheck();

			//Draw time and value fields
			position.width /= 2f;
			position.x += position.width;
			EditorGUI.MultiFloatField(position, new GUIContent[2] { new GUIContent("Duration"), new GUIContent("Value") }, floatFields);

			//Create backing curve, this is to prevent EditorGUI.CurveField to override the manipulations we do here
			AnimationCurve backingCurve = new AnimationCurve();
			backingCurve.CopyFrom(property.animationCurveValue);

			if (EditorGUI.EndChangeCheck())
			{
				//apply changes to backing curve
				float xScale = floatFields[0] / lastKeyframe.time;
				float yScale = floatFields[1] / lastKeyframe.value;
				backingCurve = backingCurve.ScaleCurve(xScale, yScale);
			}

			//Draw curve field
			position.x -= position.width;
			property.animationCurveValue = EditorGUI.CurveField(position, propertyLabel, backingCurve);
			EditorGUI.EndProperty();
		}
	}
}
