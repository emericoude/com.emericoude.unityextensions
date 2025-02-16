using UnityEditor;
using UnityEngine;

namespace Emericoude.Tests.Editor
{
    [CustomEditor(typeof(TimeManagerTests))]
    public class TimeManagerTestsEditor : UnityEditor.Editor
    {
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            
            var timeManagerTests = (TimeManagerTests) this.target;
            if (GUILayout.Button("Trigger Effect A"))
            {
                timeManagerTests.TriggerEffectA();
            }
            if (GUILayout.Button("Trigger Effect B"))
            {
                timeManagerTests.TriggerEffectB();
            }
            if (GUILayout.Button("Trigger Effect C"))
            {
                timeManagerTests.TriggerEffectC();
            }
            if (GUILayout.Button("Stop Effect A"))
            {
                timeManagerTests.StopEffectA();
            }
            if (GUILayout.Button("Stop Effect B"))
            {
                timeManagerTests.StopEffectB();
            }
            if (GUILayout.Button("Stop Effect C"))
            {
                timeManagerTests.StopEffectC();
            }
        }
    }
}