using UnityEditor;

using UnityEngine;
using UnityEngine.UIElements;

namespace Emericoude.CustomEditors
{
    [CustomPropertyDrawer(typeof(RadiansDisplayedAsDegreesAttribute))]
    public class RadiansDisplayedAsDegreesAttributeDrawer : PropertyDrawer
    {
        public override VisualElement CreatePropertyGUI(SerializedProperty property) {
            if (property.propertyType != SerializedPropertyType.Float) {
                return EditorStyles.CreateWarningLabel($"<b>{ObjectNames.NicifyVariableName(nameof(RadiansDisplayedAsDegreesAttribute))}</b> for field <b>{property.displayName}</b> can only be used on float fields.");
            }
            
            return new RadiansAsDegreeField(property);
        }

        private class RadiansAsDegreeField : VisualElement
        {
            private readonly SerializedProperty property;
            
            public RadiansAsDegreeField(SerializedProperty property) {
                this.property = property;

                var floatField = new FloatField(this.property.displayName) {
                    tooltip = this.property.tooltip,
                    value = Mathf.Rad2Deg * this.property.floatValue,
                    style = { unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft) }
                };
                floatField.RegisterValueChangedCallback(this.OnValueChanged);
                floatField.AddSuffix("\u00b0", "Displayed as degrees, but internally stored in radians.");
                this.Add(floatField);
            }

            public void OnValueChanged(ChangeEvent<float> onChange) {
                this.property.floatValue = Mathf.Deg2Rad * onChange.newValue;
                this.property.serializedObject.ApplyModifiedProperties();
            }
        }
    }
}