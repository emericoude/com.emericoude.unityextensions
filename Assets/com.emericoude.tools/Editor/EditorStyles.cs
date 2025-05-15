using System;
using Emericoude.Helpers;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;
using Object = UnityEngine.Object;

namespace Emericoude.CustomEditors
{
    public static class EditorStyles
    {
        public static Color CustomDrawerBackgroundColor => new Color(0.19f, 0.19f, 0.19f);

        #region Custom Editor/Drawer Styles
        
        public static void ApplyCustomEditorRootStyle(this VisualElement visualElement)
        {
            visualElement.style.SetPadding(8);
        }
        
        public static void ApplyCustomDrawerRootStyle(this VisualElement visualElement)
        {
            visualElement.style.backgroundColor = EditorStyles.CustomDrawerBackgroundColor;
            visualElement.style.SetBorderRadius(8);
            visualElement.style.SetPadding(8);
            visualElement.style.SetMargin(4);
        }

        public static Label AddCustomDrawerHeader(this VisualElement visualElement, string value = "",
            string tooltip = "")
        {
            var headerLabel = new Label($"<b>{value}")
            {
                tooltip = tooltip,
                style =
                {
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter),
                    paddingBottom = 4
                },
                enableRichText = true
            };
            visualElement.Add(headerLabel);
            return headerLabel;
        }
        
        #endregion
        #region Custom Groups
        
        public static Foldout AddEditorFoldoutContainer(this VisualElement visualElement, bool defaultFoldoutValue, string label = "", string tooltip = "", EventCallback<ChangeEvent<bool>> onValueChanged = null)
        {
            var foldout = new Foldout
            {
                text = $"<b>{label}</b> <i>(Click to Expand)</i>",
                tooltip = tooltip,
                value = defaultFoldoutValue
            };
            
            foldout.ApplyCustomDrawerRootStyle();
            foldout.style.unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleCenter);
            foldout.Q<VisualElement>("unity-checkmark").visible = false;
            foldout.Q<VisualElement>("unity-content").style.marginLeft = 0f;
            foldout.Q<Label>().style.paddingBottom = 4;
            
            if (onValueChanged != null)
            {
                foldout.Q<Toggle>().RegisterValueChangedCallback<bool>(onValueChanged);
            }

            visualElement.Add(foldout);
            return foldout;
        }
        
        #endregion
        #region Add Field Helpers
        
        public static EnumField AddEnumField(this VisualElement visualElement, SerializedProperty property, EventCallback<ChangeEvent<Enum>> onValueChanged = null)
        {
            var enumField = new EnumField(property.displayName)
            {
                tooltip = property.tooltip,
                style =
                {
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft)
                }
            };
            enumField.BindProperty(property);
            if (onValueChanged != null)
            {
                enumField.RegisterValueChangedCallback(onValueChanged);
            }
            visualElement.Add(enumField);
            return enumField;
        }

        public static Toggle AddToggleField(this VisualElement visualElement, SerializedProperty property, EventCallback<ChangeEvent<bool>> onValueChanged = null)
        {
            var toggleField = new Toggle(property.displayName)
            {
                tooltip = property.tooltip,
                style =
                {
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft)
                }
            };
            toggleField.BindProperty(property);
            if (onValueChanged != null)
            {
                toggleField.RegisterValueChangedCallback(onValueChanged);
            }

            visualElement.Add(toggleField);
            return toggleField;
        }

        public static ObjectField AddObjectField<T>(this VisualElement visualElement, SerializedProperty property, bool allowSceneObjects = true, EventCallback<ChangeEvent<Object>> onValueChanged = null) where T : Object
        {
            var objectField = new ObjectField(property.displayName)
            {
                tooltip = property.tooltip,
                objectType = typeof(T),
                allowSceneObjects = allowSceneObjects,
                style =
                {
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft)
                }
            };
            objectField.BindProperty(property);
            if (onValueChanged != null)
            {
                objectField.RegisterValueChangedCallback(onValueChanged);
            }
            visualElement.Add(objectField);
            return objectField;
        }

        public static PropertyField AddPropertyField(this VisualElement visualElement, SerializedProperty property, EventCallback<SerializedPropertyChangeEvent> onValueChanged = null)
        {
            var propertyField = new PropertyField(property, property.displayName)
            {
                tooltip = property.tooltip,
                style =
                {
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft)
                }
            };
            if (onValueChanged != null)
            {
                propertyField.RegisterValueChangeCallback(onValueChanged);
            }
            
            visualElement.Add(propertyField);
            return propertyField;
        }

        public static Vector4Field AddVector4Field(
            this VisualElement visualElement, 
            SerializedProperty property, 
            string xLabel = "x",
            string yLabel = "y",
            string zLabel = "z",
            string wLabel = "w",
            EventCallback<ChangeEvent<Vector4>> onValueChanged = null) {
            var vector4Field = new Vector4Field(property.displayName) {
                tooltip = property.tooltip,
                style =
                {
                    unityTextAlign = new StyleEnum<TextAnchor>(TextAnchor.MiddleLeft)
                }
            };
            vector4Field.BindProperty(property);
            if (onValueChanged != null)
            {
                vector4Field.RegisterValueChangedCallback(onValueChanged);
            }
            vector4Field.Q<FloatField>("unity-x-input").label = xLabel;
            vector4Field.Q<FloatField>("unity-y-input").label = yLabel;
            vector4Field.Q<FloatField>("unity-z-input").label = zLabel;
            vector4Field.Q<FloatField>("unity-w-input").label = wLabel;
            visualElement.Add(vector4Field);
            return vector4Field;
        }
        
        #endregion
    }
}