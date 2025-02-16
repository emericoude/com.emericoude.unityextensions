using UnityEditor;
using UnityEngine;

namespace Emericoude.CustomEditors
{
    [CustomPropertyDrawer(typeof(TimeEffect))]
    public class TimeEffectDrawer : PropertyDrawer
    {
        public bool foldout = true;
        
        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label) {
            EditorGUI.BeginProperty(position, label, property);
            
            var priority = property.FindPropertyRelative("priority");
            var infinite = property.FindPropertyRelative("infinite");
            var duration = property.FindPropertyRelative("duration");
            var timeScale = property.FindPropertyRelative("timeScale");
            var deltaTime = property.FindPropertyRelative("deltaTime");
            var curve = property.FindPropertyRelative("curve");
            
            GUIContent labelContent = new GUIContent(label.text, label.tooltip); //not sure why, but I need to save this before using DropShadowLabel.
            
            Rect windowRect = new Rect(position.x, position.y, position.width, position.height);
            EditorGUI.DropShadowLabel(windowRect, "", "window");
            
            position.height = EditorGUIUtility.singleLineHeight;
            position.x += 16;
            position.width -= 32;
            
            this.foldout = EditorGUI.BeginFoldoutHeaderGroup(position, this.foldout, labelContent, EditorStyles.foldout);
            
            if (this.foldout)
            {
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing * 2;
                EditorGUI.PropertyField(position, priority, new GUIContent("Priority", "The priority of this effect. Effects with higher priority will always be in effect over lower priority ones."));
            
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(position, infinite, new GUIContent("Infinite", "If true, the effect will never stop until manually removed."));
            
                if (!infinite.boolValue)
                {
                    position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                    EditorGUI.PropertyField(position, duration, new GUIContent("Duration", "The duration of this effect. The effect will stop after this duration."));
                }
            
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(position, timeScale, new GUIContent("Target Time Scale", "The time scale for this effect. This is the time scale value applied when the curve's value is at 1."));
            
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(position, deltaTime, new GUIContent("Delta Time", "The delta time scale of this effect. You usually want 'Delta Time unscaled Except Pause', unless this effect is a pause, in which case you want 'Delta Time'."));
            
                position.y += EditorGUIUtility.singleLineHeight + EditorGUIUtility.standardVerticalSpacing;
                EditorGUI.PropertyField(position, curve, new GUIContent("Curve", "The curve that defines the time scale value over time. A value of 0 equals normal timeScale, a value of 1 equals this effect's target timeScale. Note that this is used in an unclamped manner."));
            }
            
            EditorGUI.EndFoldoutHeaderGroup();
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            if (!this.foldout) return EditorGUIUtility.singleLineHeight;
            
            var priority = property.FindPropertyRelative("priority");
            var infinite = property.FindPropertyRelative("infinite");
            var duration = property.FindPropertyRelative("duration");
            var timeScale = property.FindPropertyRelative("timeScale");
            var deltaTime = property.FindPropertyRelative("deltaTime");
            var curve = property.FindPropertyRelative("curve");
            float height = EditorGUI.GetPropertyHeight(priority) 
                           + EditorGUI.GetPropertyHeight(infinite) 
                           + EditorGUI.GetPropertyHeight(timeScale) 
                           + EditorGUI.GetPropertyHeight(deltaTime) 
                           + EditorGUI.GetPropertyHeight(curve) 
                           + EditorGUIUtility.singleLineHeight // label
                           + EditorGUIUtility.standardVerticalSpacing * 8;
            
            if (!infinite.boolValue) height += EditorGUI.GetPropertyHeight(duration) + EditorGUIUtility.standardVerticalSpacing;
            return height;
        }
    }
}