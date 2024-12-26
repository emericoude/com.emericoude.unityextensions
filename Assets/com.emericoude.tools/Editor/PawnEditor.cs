using UnityEditor;
using UnityEngine.InputSystem;
using Emericoude.Pawns;

namespace Emericoude.CustomEditors
{
    [CustomEditor(typeof(Pawn), true)]
    public class PawnEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            this.serializedObject.Update();
            
            // Get the action asset property
            var actionAssetProperty = this.serializedObject.FindProperty("inputActionAsset");
            EditorGUILayout.PropertyField(actionAssetProperty);

            // Get the default action map property
            var defaultActionMapProperty = this.serializedObject.FindProperty("defaultActionMap");
            if (actionAssetProperty.objectReferenceValue != null)
            {
                //Fetch action maps
                var actionAsset = (InputActionAsset)actionAssetProperty.objectReferenceValue;
                string[] actionMapNames = new string[actionAsset.actionMaps.Count];
                for (int i = 0; i < actionAsset.actionMaps.Count; i++)
                {
                    actionMapNames[i] = actionAsset.actionMaps[i].name;
                }

                // Find the index of the current action map in the list
                int currentIndex = System.Array.IndexOf(actionMapNames, defaultActionMapProperty.stringValue);
                int selectedIndex = EditorGUILayout.Popup("Default Action Map", currentIndex, actionMapNames);
                if (selectedIndex != currentIndex)
                {
                    defaultActionMapProperty.stringValue = actionMapNames[selectedIndex];
                }
            }

            // Draw other properties excluding script, actionAsset, and defaultActionMap
            DrawPropertiesExcluding(this.serializedObject, "m_Script", "inputActionAsset", "defaultActionMap");
            this.serializedObject.ApplyModifiedProperties();
        }
    }
}