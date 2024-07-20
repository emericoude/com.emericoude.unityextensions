using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Emericoude
{
    /// <summary> Creates a dropdown of types deriving from the given type, then creates and serializes an instance of this type. <para/>
    /// Allowing you to select any type that inherits the given type, as long as it is non-abstract and serializable. </summary>
    /// <remarks> USE IN CUNJONCTION WITH THE [<see cref="SerializeReference"/>] ATTRIBUTE, as this only supports <see cref="SerializedPropertyType.ManagedReference"/>s. </remarks>
    [AttributeUsage(AttributeTargets.Field)] 
    public class TypeFilterAttribute : PropertyAttribute
    {
        public readonly Type filteredType;

        public TypeFilterAttribute(Type type)
        {
            this.filteredType = type;
        }
    }
    
#if UNITY_EDITOR && !ODIN_INSPECTOR
    /// <summary> Editor drawer for type filters. </summary>
    [CustomPropertyDrawer(typeof(TypeFilterAttribute))]
    public class TypeFilterDrawer : PropertyDrawer 
    {
        private static readonly Dictionary<Type, Type[]> CachedDerivedTypes = new Dictionary<Type, Type[]>();
        private static readonly Dictionary<Type, string[]> CachedDisplayNames = new Dictionary<Type, string[]>();
        private static readonly Dictionary<Type, FieldInfo[]> CachedSerializedFields = new Dictionary<Type, FieldInfo[]>();

        private const float FieldVerticalPositionAsSingleLineHeightRatio = 1.5f;
        private const float FieldIndentationAsWidthRatio = 0.05f;
        private const float FieldLabelWidthAsWidthRatio = 0.25f;

        public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
        {
            EditorGUI.BeginProperty(position, label, property);
            EditorGUI.BeginChangeCheck();
            
            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                TypeFilterAttribute typeFilterAttribute = attribute as TypeFilterAttribute;
                
                //Fetch types
                var baseType = typeFilterAttribute.filteredType;
                var allTypes = GetDerivedTypes(baseType);

                //Get the currently selected type index, used for editor drawing
                var selectedType = property.managedReferenceValue?.GetType();
                int selectedIndex = Array.IndexOf(allTypes, selectedType);

                //Draw the dropdown, and store the selected index
                selectedIndex = EditorGUI.Popup(
                    new Rect(
                        position.x, position.y, 
                        position.width, EditorGUIUtility.singleLineHeight), 
                    selectedIndex, 
                    GetDerivedTypesDisplayNames(baseType)
                );

                //Ensure our selection is within the possible range
                selectedIndex = Mathf.Clamp(selectedIndex, 0, allTypes.Length - 1);
                
                //If we've selected a new type, create an instance of that type.
                if (selectedType != allTypes[selectedIndex])
                {
                    selectedType = allTypes[selectedIndex];

                    try
                    {
                        object newInstance = Activator.CreateInstance(selectedType);
                        property.managedReferenceValue = newInstance;
                    }
                    catch (Exception exception)
                    {
                        Debug.Log($"Error creating or assigning instance of {selectedType}: {exception.Message}\n{exception.StackTrace}");
                    }
                    
                }
                
                //Draw the field's content
                var fields = this.GetSerializableFields(selectedType);
                float fieldsYPosition = EditorGUIUtility.singleLineHeight * FieldVerticalPositionAsSingleLineHeightRatio;
                float fieldsIndentation = position.width *= FieldIndentationAsWidthRatio;
                float minLabelWidth = position.width * FieldLabelWidthAsWidthRatio;
                EditorGUIUtility.labelWidth = GetLargestLabelWidth(fields, minLabelWidth);
                foreach (var field in fields)
                {
                    var fieldProperty = property.serializedObject.FindProperty(property.propertyPath).FindPropertyRelative(field.Name);
                    float fieldPropertyHeight = EditorGUI.GetPropertyHeight(fieldProperty);
                    Vector2 fieldPosition = new Vector2(position.x + fieldsIndentation, position.y + fieldsYPosition);
                    Vector2 fieldDimensions = new Vector2(position.width - fieldsIndentation, fieldPropertyHeight);
                    
                    EditorGUI.PropertyField(new Rect(fieldPosition, fieldDimensions), fieldProperty, true);
                    fieldsYPosition += fieldPropertyHeight + EditorGUIUtility.standardVerticalSpacing;
                }
            }
            else
            {
                EditorGUI.LabelField(position, label.text, $"Use TypeFilter with managed references. Not valid with {property.propertyType}");
            }

            if (EditorGUI.EndChangeCheck())
            {
                property.serializedObject.ApplyModifiedProperties();
            }
            
            EditorGUI.EndProperty();
        }

        public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
        {
            float height = EditorGUIUtility.singleLineHeight * FieldVerticalPositionAsSingleLineHeightRatio;
            if (property.propertyType == SerializedPropertyType.ManagedReference)
            {
                var selectedType = property.managedReferenceValue?.GetType();
                if (selectedType != null)
                {
                    var fields = GetSerializableFields(selectedType);
                    foreach (var field in fields)
                    {
                        var fieldProperty = property.serializedObject.FindProperty(property.propertyPath).FindPropertyRelative(field.Name);
                        height += EditorGUI.GetPropertyHeight(fieldProperty) + EditorGUIUtility.standardVerticalSpacing;
                    }
                }
            }

            return height;
        }

        /// <summary> Fetches and caches all types deriving (inheriting) from the base type. </summary>
        /// <param name="baseType"> The type source for getting derived types. </param>
        /// <returns> Types deriving from <paramref name="baseType"/>'s <see cref="Type"/>. </returns>
        private Type[] GetDerivedTypes(Type baseType)
        {
            if (CachedDerivedTypes.TryGetValue(baseType, out Type[] derivedTypes))
            {
                return derivedTypes;
            }
            
            var newDerivedTypes = new List<Type>();
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();
            foreach (var assembly in assemblies)
            {
                var types = assembly.GetTypes();
                foreach (var type in types)
                {
                    if (baseType.IsAssignableFrom(type) && !type.IsAbstract && type.IsSerializable)
                    {
                        newDerivedTypes.Add(type);
                    }
                }
            }

            derivedTypes = newDerivedTypes.ToArray();
            CachedDerivedTypes[baseType] = derivedTypes; //cache for performance
            return derivedTypes;
        }

        /// <summary> Nicifies, then caches all deriving types' display names. </summary>
        /// <param name="type"> The type source for getting derived types. </param>
        /// <returns> A display name array of types deriving from <paramref name="type"/>'s <see cref="Type"/>. </returns>
        private string[] GetDerivedTypesDisplayNames(Type type)
        {
            if (CachedDisplayNames.TryGetValue(type, out string[] displayNames))
            {
                return displayNames;
            }

            var derivedTypes = GetDerivedTypes(type);
            displayNames = new string[derivedTypes.Length];
            for (int i = 0; i < derivedTypes.Length; i++) {
                displayNames[i] = ObjectNames.NicifyVariableName(derivedTypes[i].Name);
            }

            CachedDisplayNames[type] = displayNames;
            return displayNames;
        }

        /// <summary> Fetches and caches all serializable fields for a type. </summary>
        /// <param name="type"> The type target. </param>
        /// <returns> All serializable fields for the given type. </returns>
        private FieldInfo[] GetSerializableFields(Type type)
        {
            if (CachedSerializedFields.TryGetValue(type, out FieldInfo[] fields))
            {
                return fields;
            }

            fields = type.GetFields(BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);
            fields = fields.Where(f => f.IsPublic || Attribute.IsDefined(f, typeof(SerializeField))).ToArray();
            CachedSerializedFields[type] = fields;
            return fields;
        }

        /// <summary> Calculates the largest label from an array of fields. </summary>
        /// <param name="fields"> Fields of a property to check among. </param>
        /// <param name="minWidth"> The minimum width that could be considered. </param>
        /// <returns> The width of the largest label. </returns>
        private float GetLargestLabelWidth(FieldInfo[] fields, float minWidth = 0f)
        {
            float largestWidth = minWidth;
            foreach (var field in fields)
            {
                float width = EditorStyles.largeLabel.CalcSize(new GUIContent(field.Name)).x;
                if (width > largestWidth) largestWidth = width;
            }
            
            return largestWidth;
        }
    }
#endif
}
