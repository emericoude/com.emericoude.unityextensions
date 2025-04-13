using System;
using Emericoude.Helpers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;

using static Emericoude.Navigation3D;

namespace Emericoude.CustomEditors
{
    [CustomPropertyDrawer(typeof(Navigation3D))]
    public class Navigation3DDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property)
        {
            return new Navigation3DVisualElement(property);
        }

        private sealed class Navigation3DVisualElement : VisualElement
        {
            private readonly VisualElement settingsContainer;
            private readonly VisualElement automaticSettingsContainer;
            private readonly VisualElement sphereCastSettingsContainer;
            private readonly VisualElement explicitSettingsContainer;
            
            public Navigation3DVisualElement(SerializedProperty property) : base()
            {
                this.ApplyCustomDrawerRootStyle();
                this.AddCustomDrawerHeader(property.displayName, property.tooltip);
                
                this.AddEnumField(property.FindPropertyRelative("NavigationMode"), this.HandleNavigationModeValueChange);

                this.settingsContainer = this.AddEmptyVisualElement();
                this.settingsContainer.AddPropertyField(property.FindPropertyRelative("NavigationAxisMode"));

                this.automaticSettingsContainer = this.settingsContainer.AddEmptyVisualElement();
                this.automaticSettingsContainer.AddToggleField(property.FindPropertyRelative("AutomateSphereCastCalculationFromColliderBounds"), this.HandleCalcAutomationValueChange);

                this.sphereCastSettingsContainer = this.automaticSettingsContainer.AddEmptyVisualElement();
                this.sphereCastSettingsContainer.AddPropertyField(property.FindPropertyRelative("SphereCastRadius"));
                this.sphereCastSettingsContainer.AddPropertyField(property.FindPropertyRelative("SphereCastMaximumDistance"));
                this.sphereCastSettingsContainer.AddPropertyField(property.FindPropertyRelative("SphereCastMaximumHits"));
                this.sphereCastSettingsContainer.AddPropertyField(property.FindPropertyRelative("SphereCastLayer"));

                this.explicitSettingsContainer = this.settingsContainer.AddEmptyVisualElement();
                this.explicitSettingsContainer.AddPropertyField(property.FindPropertyRelative("SelectOnUp"));
                this.explicitSettingsContainer.AddPropertyField(property.FindPropertyRelative("SelectOnDown"));
                this.explicitSettingsContainer.AddPropertyField(property.FindPropertyRelative("SelectOnLeft"));
                this.explicitSettingsContainer.AddPropertyField(property.FindPropertyRelative("SelectOnRight"));
            }

            private void HandleNavigationModeValueChange(ChangeEvent<Enum> changeEvent)
            {
                var newModeValue = (Mode)changeEvent.newValue;
                this.settingsContainer.style.SetDisplay(newModeValue is not Mode.None);
                this.automaticSettingsContainer.style.SetDisplay(newModeValue is Mode.Automatic or Mode.Horizontal or Mode.Vertical);
                this.explicitSettingsContainer.style.SetDisplay(newModeValue is Mode.Explicit);
            }
            
            private void HandleCalcAutomationValueChange(ChangeEvent<bool> changeEvent)
            {
                this.sphereCastSettingsContainer.style.SetDisplay(!changeEvent.newValue);
            }
        }
    }
}