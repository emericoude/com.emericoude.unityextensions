using System.Collections;
using System.Collections.Generic;
using System.Linq;

using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

using Emericoude.UnityExtensions;

namespace Emericoude.UnityExtensions.Editor
{
	[CustomPropertyDrawer(typeof(BetterCurveFieldAttribute))]
	public class BetterCurveDrawer : PropertyDrawer
	{
		public float[] floatFields = new float[2] { 1.0f , 1.0f };

		public override void OnGUI (Rect position, SerializedProperty property, GUIContent label)
		{
			GUIContent propertyLabel = EditorGUI.BeginProperty(position, label, property);

			position.width /= 2.0f;

			//Draw float fields
			Keyframe lastKeyframe = property.animationCurveValue.keys.LastOrDefault();
			float[] floatFields = new float[2]
			{
				lastKeyframe.time,
				lastKeyframe.value
			};

			EditorGUI.BeginChangeCheck();

			position.x += position.width;
			EditorGUI.MultiFloatField(
				position, 
				GUIContent.none,
				new GUIContent[2] { new GUIContent("Duration"), new GUIContent("Height") },  
				floatFields
			);

			//Apply changes
			AnimationCurve curve = new AnimationCurve();
			curve.CopyFrom(property.animationCurveValue);
			if (EditorGUI.EndChangeCheck())
			{
				//lastKeyframe.time = floatFields[0];
				//lastKeyframe.value = floatFields[1];
				//curve.MoveKey(property.animationCurveValue.keys.Length - 1, lastKeyframe);

				Vector2 distanceMoved = new Vector2(
					floatFields[0] - lastKeyframe.time,
					floatFields[1] - lastKeyframe.value
				);

				for (int i = property.animationCurveValue.keys.Length - 1; i > 0; i--)
				{
					Keyframe key = property.animationCurveValue.keys[i];
					if (key.time == 0) continue;
					if (key.value == 0) continue;

					Vector2 relativePosition = new Vector2(
						Mathf.InverseLerp(0, lastKeyframe.time, key.time),
						Mathf.InverseLerp(0, lastKeyframe.value, key.value)
					);

					key.time = Mathf.Lerp(0, lastKeyframe.time + distanceMoved.x, relativePosition.x);
					key.value = Mathf.Lerp(0, lastKeyframe.value + distanceMoved.y, relativePosition.y);

					curve.MoveKey(i, key);
				}
			}

			position.x -= position.width;
			property.animationCurveValue = EditorGUI.CurveField(position, propertyLabel, curve);
			EditorGUI.EndProperty();
		}
	}
}
