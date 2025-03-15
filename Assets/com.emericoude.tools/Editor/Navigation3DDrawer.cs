using System;
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
                this.style.backgroundColor = new Color(0.19f, 0.19f, 0.19f);
                this.style.borderBottomLeftRadius= 4;
                this.style.borderBottomLeftRadius= 4;
                this.style.borderBottomLeftRadius= 4;
                this.style.borderBottomLeftRadius= 4;
                this.style.paddingBottom = 8;
                this.style.paddingTop = 8;
                this.style.paddingLeft = 8;
                this.style.paddingRight = 8;

                var labelField = new Label($"<b>{property.displayName}</b>");
                labelField.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
                labelField.enableRichText = true;
                labelField.style.paddingBottom = 4;
                this.Add(labelField);
                
                var navigationModeProperty = property.FindPropertyRelative("NavigationMode");
                var navigationMode = new EnumField(navigationModeProperty.displayName);
                navigationMode.BindProperty(navigationModeProperty);
                navigationMode.RegisterValueChangedCallback(this.HandleNavigationModeValueChange);
                this.Add(navigationMode);
                
                this.settingsContainer = new VisualElement();
                this.Add(this.settingsContainer);
                
                var navigationAxisMode = new PropertyField(property.FindPropertyRelative("NavigationAxisMode"));
                this.settingsContainer.Add(navigationAxisMode);

                this.automaticSettingsContainer = new VisualElement();
                this.settingsContainer.Add(this.automaticSettingsContainer);
                
                var automateSphereCastCalculationsProperty = property.FindPropertyRelative("AutomateSphereCastCalculationFromColliderBounds");
                var automateSphereCastCalculations = new Toggle(automateSphereCastCalculationsProperty.displayName);
                automateSphereCastCalculations.BindProperty(automateSphereCastCalculationsProperty);
                automateSphereCastCalculations.RegisterValueChangedCallback<bool>(this.HandleCalcAutomationValueChange);
                
                var sphereCastRadius = new PropertyField(property.FindPropertyRelative("SphereCastRadius"));
                var sphereCastMaximumDistance = new PropertyField(property.FindPropertyRelative("SphereCastMaximumDistance"));
                var sphereCastMaximumHits = new PropertyField(property.FindPropertyRelative("SphereCastMaximumHits"));
                var sphereCastLayer = new PropertyField(property.FindPropertyRelative("SphereCastLayer"));

                this.sphereCastSettingsContainer = new VisualElement();
                this.sphereCastSettingsContainer.Add(sphereCastRadius);
                this.sphereCastSettingsContainer.Add(sphereCastMaximumDistance);
                this.sphereCastSettingsContainer.Add(sphereCastMaximumHits);
                
                this.automaticSettingsContainer.Add(automateSphereCastCalculations);
                this.automaticSettingsContainer.Add(this.sphereCastSettingsContainer);
                this.automaticSettingsContainer.Add(sphereCastLayer);

                this.explicitSettingsContainer = new VisualElement();
                this.settingsContainer.Add(this.explicitSettingsContainer);
                
                var selectOnUp = new PropertyField(property.FindPropertyRelative("SelectOnUp"));
                var selectOnDown = new PropertyField(property.FindPropertyRelative("SelectOnDown"));
                var selectOnLeft = new PropertyField(property.FindPropertyRelative("SelectOnLeft"));
                var selectOnRight = new PropertyField(property.FindPropertyRelative("SelectOnRight"));
                this.explicitSettingsContainer.Add(selectOnUp);
                this.explicitSettingsContainer.Add(selectOnDown);
                this.explicitSettingsContainer.Add(selectOnLeft);
                this.explicitSettingsContainer.Add(selectOnRight);
            }

            private void HandleNavigationModeValueChange(ChangeEvent<Enum> changeEvent)
            {
                Mode newModeValue = (Mode)changeEvent.newValue;
                this.UpdateVisibility(this.settingsContainer, newModeValue is not Mode.None);
                this.UpdateVisibility(this.automaticSettingsContainer, newModeValue is Mode.Automatic or Mode.Horizontal or Mode.Vertical);
                this.UpdateVisibility(this.explicitSettingsContainer, newModeValue is Mode.Explicit);
            }
            
            private void HandleCalcAutomationValueChange(ChangeEvent<bool> changeEvent)
            {
                this.UpdateVisibility(this.sphereCastSettingsContainer, !changeEvent.newValue);
            }

            private void UpdateVisibility(VisualElement visualElement, bool value)
            {
                visualElement.style.display = value ? DisplayStyle.Flex : DisplayStyle.None;
            }
        }
    }
}