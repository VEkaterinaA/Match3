using Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace HiddenLightEditor.PropertyDrawers.Attributes
{
	[CustomPropertyDrawer(typeof(GetComponentAttribute))]
	internal sealed class GetComponentAttributePropertyDrawer : PropertyDrawer
	{
		private GetComponentAttribute GetComponentAttribute => (attribute as GetComponentAttribute);

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
					var targetType = GetComponentAttribute.OverrideTypeToAdd ?? fieldInfo.FieldType;

					var targetComponentReference = component.gameObject.GetComponent(targetType);

					if (GetComponentAttribute.AddComponentIfNotFound && !targetComponentReference)
					{
						targetComponentReference = component.gameObject.AddComponent(targetType);
					}

					serializedProperty.objectReferenceValue = targetComponentReference;
					serializedProperty.serializedObject.ApplyModifiedProperties();
				}
			}

			EditorGUI.ObjectField(position, serializedProperty, label);
		}
	}
}