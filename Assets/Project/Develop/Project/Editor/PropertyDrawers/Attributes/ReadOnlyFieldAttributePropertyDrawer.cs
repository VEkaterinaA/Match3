using Runtime.Attributes;
using UnityEditor;
using UnityEngine;

namespace HiddenLightEditor.PropertyDrawers.Attributes
{
	[CustomPropertyDrawer(typeof(ReadOnlyFieldAttribute))]
	internal sealed class ReadOnlyFieldAttributePropertyDrawer : PropertyDrawer
	{
		public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
		{
			GUI.enabled = false;

			EditorGUI.PropertyField(position, property, label);

			GUI.enabled = true;
		}
	}
}