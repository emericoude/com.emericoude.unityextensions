using UnityEditor;
using UnityEngine;
using Emericoude.StateMachine;

namespace Emericoude.CustomEditors
{
    //TODO: make a custom drawer for state machine instead.
    
    [CustomEditor(typeof(StateMachineMonoBehaviour), true)]
    public class StateMachineMonoBehaviourEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            serializedObject.Update();
            var stateMono = (StateMonoBehaviour)target;
            var activeColor = stateMono.IsActive ? new Color(0.22f, 0.28f, 0.22f) : new Color(0.28f, 0.22f, 0.22f);
        
            var screenRect = GUILayoutUtility.GetRect(1, 1);
            var vertRect = EditorGUILayout.BeginVertical();
            EditorGUI.DrawRect(new Rect(screenRect.x - 30, screenRect.y - EditorGUIUtility.standardVerticalSpacing * 2f, screenRect.width + 45, vertRect.height + EditorGUIUtility.singleLineHeight), activeColor);
            base.OnInspectorGUI();

            var stateMachineMonoBehaviour = (StateMachineMonoBehaviour)this.target;
            var stateMachine = stateMachineMonoBehaviour.StateMachine;

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("State Machine Transitions", UnityEditor.EditorStyles.boldLabel);

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("State Machine transitions are only visible in Play Mode.", MessageType.Info);
            }
            
            var transitionStyle = new GUIStyle(GUI.skin.label)
            {
                richText = true
            };
            
            var currentState = stateMachine.GetCurrentStateNode();
            
            //draw from any transitions
            foreach (var transition in stateMachine.GetFromAnyTransitions()) {
                var toState = transition.To.GetType().Name;
                var isSelf = currentState.State == transition.To;
                var fromStateColorRich = (transition.CanTransitionToSelf || currentState.State != transition.To) ? "green" : "red";
                var toStateColorRich = transition.To.GetType() == currentState.State.GetType() ? "green" : "white";
                EditorGUILayout.LabelField(
                    $"<color={fromStateColorRich}>{(isSelf ? "SELF" : "ANY")}</color> " + $"-> <color={toStateColorRich}>{toState}</color>",
                    transitionStyle
                );
            }
            
            EditorGUILayout.Space();
            
            //draw transitions
            foreach (var node in stateMachine.GetStateNodes()) {
                var transitions = node.Value.Transitions;
                foreach (var transition in transitions) {
                    var fromState = node.Key.Name;
                    var toState = transition.To.GetType().Name;
                    var fromStateColorRich =  node.Key == currentState.State.GetType() ? "green" : "white";
                    var toStateColorRich = transition.To.GetType() == currentState.State.GetType() ? "green" : "white";
                    EditorGUILayout.LabelField(
                        $"<color={fromStateColorRich}>{fromState}</color> " + $"-> <color={toStateColorRich}>{toState}</color>",
                        transitionStyle
                    );
                }
            }
            
            EditorGUILayout.EndVertical();
        }
    }
}