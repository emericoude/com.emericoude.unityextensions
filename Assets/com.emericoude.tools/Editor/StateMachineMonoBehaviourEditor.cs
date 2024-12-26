using UnityEditor;
using UnityEngine;
using Emericoude.StateMachine;

namespace Emericoude.CustomEditors
{
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
            EditorGUILayout.LabelField("State Machine Transitions", EditorStyles.boldLabel);

            if (!Application.isPlaying)
            {
                EditorGUILayout.HelpBox("State Machine transitions are only visible in Play Mode.", MessageType.Info);
            }
            
            var transitionStyle = new GUIStyle(GUI.skin.label)
            {
                richText = true
            };
            
            var currentState = stateMachine.GetActiveNode();
            foreach (var node in stateMachine.GetNodes())
            {
                var transitions = node.Value.Transitions;
                foreach (var transition in transitions)
                {
                    var fromState = node.Key.Name;
                    var toState = transition.To.GetType().Name;
                    var isConditionMet = transition.Condition.Evaluate();
                    var isFromStateCurrent = node.Key == currentState.State.GetType();
                    var isToStateCurrent = transition.To.GetType() == currentState.State.GetType();
                    var fromStateColorRich = isFromStateCurrent ? "green" : "white";
                    var toStateColorRich = isToStateCurrent ? "green" : "white";
                    
                    EditorGUILayout.LabelField($"<color={fromStateColorRich}>{fromState}</color> " +
                                               $"-> <color={toStateColorRich}>{toState}</color>  " +
                                               $":  <b>{isConditionMet}</b>", 
                                                transitionStyle);
                }
            }
            
            EditorGUILayout.EndVertical();

            if (Application.isPlaying)
            {
                //Force a repaint
                Repaint();
                
                //Force queue a player loop update to refresh the inspector
                EditorApplication.QueuePlayerLoopUpdate();
            }
        }
    }
}