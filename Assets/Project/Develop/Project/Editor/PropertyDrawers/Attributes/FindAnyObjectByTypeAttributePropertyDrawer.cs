using Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace HiddenLightEditor.PropertyDrawers.Attributes
{
    [CustomPropertyDrawer(typeof(FindAnyObjectByTypeAttribute))]
    internal sealed class FindAnyObjectByTypeAttributePropertyDrawer : PropertyDrawer
    {
        public override void OnGUI(Rect position, SerializedProperty serializedProperty, GUIContent label)
        {
            if (!typeof(Component).IsAssignableFrom(fieldInfo.FieldType))
            {
                EditorGUI.HelpBox(position, $"[{nameof(GetComponentAttribute)}] can only be used for the component field.", MessageType.Error);
                return;
            }

            if (!serializedProperty.objectReferenceValue)
            {
                if ((serializedProperty.serializedObject.targetObject is Component component))
                {
                    var targetComponentReference = Object.FindAnyObjectByType(fieldInfo.FieldType);

                    serializedProperty.objectReferenceValue = targetComponentReference;
                    serializedProperty.serializedObject.ApplyModifiedProperties();
                }
            }

            EditorGUI.ObjectField(position, serializedProperty, label);
        }
    }
}