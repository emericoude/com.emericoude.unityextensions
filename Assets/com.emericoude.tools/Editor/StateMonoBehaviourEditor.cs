using Emericoude.StateMachine;
using UnityEditor;
using UnityEngine;

namespace Emericoude.CustomEditors
{
    [CustomEditor(typeof(StateMonoBehaviour), true)]
    public class StateMonoBehaviourEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var stateMono = (StateMonoBehaviour)target;
            var activeColor = stateMono.IsActive ? new Color(0.22f, 0.28f, 0.22f) : new Color(0.28f, 0.22f, 0.22f);

            var screenRect = GUILayoutUtility.GetRect(1, 1);
            var vertRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(
                new Rect(screenRect.x - 30, screenRect.y - EditorGUIUtility.standardVerticalSpacing * 2f,
                    screenRect.width + 45, vertRect.height + EditorGUIUtility.singleLineHeight), activeColor);
            base.OnInspectorGUI();
            EditorGUILayout.EndVertical();
        }
    }
}