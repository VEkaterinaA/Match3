using Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace HiddenLightEditor.PropertyDrawers.Attributes
{
	[CustomPropertyDrawer(typeof(GetComponentInChildrenAttribute))]
	internal sealed class GetComponentInChildrenAttributePropertyDrawer : PropertyDrawer
	{
		private GetComponentInChildrenAttribute GetComponentAttribute => (attribute as GetComponentInChildrenAttribute);

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
					var targetComponentReference = component.gameObject.GetComponentInChildren(fieldInfo.FieldType);

					if (GetComponentAttribute.AddComponentIfNotFound && !targetComponentReference)
					{
						targetComponentReference = component.gameObject.AddComponent(fieldInfo.FieldType);
					}

					serializedProperty.objectReferenceValue = targetComponentReference;
					serializedProperty.serializedObject.ApplyModifiedProperties();
				}
			}

			EditorGUI.ObjectField(position, serializedProperty, label);
		}
	}
}