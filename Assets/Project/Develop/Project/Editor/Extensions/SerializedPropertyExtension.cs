using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Object = UnityEngine.Object;

namespace HiddenLightEditor.Extensions
{
	internal static class SerializedPropertyExtension
	{
		internal static void FillByComponentsOfType<TObject>(this SerializedProperty serializedProperty)
		{
			var components = Object.FindObjectsByType<Component>(FindObjectsInactive.Include, FindObjectsSortMode.None).Where(component => (component is TObject)).ToList();

			serializedProperty.FillArray(components);
		}

		internal static void FillArray(this SerializedProperty serializedProperty, IReadOnlyList<Object> arrayItems)
		{
			serializedProperty.ClearArray();
			serializedProperty.arraySize = arrayItems.Count;

			for (var itemIndex = 0; itemIndex < serializedProperty.arraySize; itemIndex++)
			{
				serializedProperty.GetArrayElementAtIndex(itemIndex).objectReferenceValue = arrayItems[itemIndex];
			}
		}

		internal static void FillIfEmpty<TComponent>(this SerializedProperty serializedProperty, GameObject gameObject) where TComponent : Component
		{
			if (!serializedProperty.objectReferenceValue)
			{
				serializedProperty.objectReferenceValue = gameObject.GetComponent<TComponent>();
			}
		}

		internal static void FillFromChildIfEmpty<TComponent>(this SerializedProperty serializedProperty, GameObject gameObject) where TComponent : Component
		{
			if (!serializedProperty.objectReferenceValue)
			{
				serializedProperty.objectReferenceValue = gameObject.GetComponentInChildren<TComponent>();
			}
		}

		internal static void FillIfEmpty<TComponent>(this SerializedProperty serializedProperty, String gameObjectName) where TComponent : Component
		{
			if (!serializedProperty.objectReferenceValue)
			{
				serializedProperty.objectReferenceValue = GameObject.Find(gameObjectName).GetComponent<TComponent>();
			}
		}

		internal static void FillFromChildIfEmpty<TComponent>(this SerializedProperty serializedProperty, GameObject gameObject, String childGameObjectName) where TComponent : Component
		{
			if (!serializedProperty.objectReferenceValue)
			{
				var targetGameObject = gameObject.GetComponentsInChildren<Transform>().FirstOrDefault(transform => transform.name == childGameObjectName);

				if (targetGameObject)
				{
					serializedProperty.objectReferenceValue = targetGameObject.GetComponent<TComponent>();
				}
			}
		}
	}
}