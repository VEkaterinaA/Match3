using System.Linq;
using Runtime.Extensions.System;
using Runtime.MonoBehaviours;
using Runtime.MonoBehaviours.LifetimeScopes.Core;
using HiddenLightEditor.Extensions;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HiddenLightEditor.CustomEditors
{
	[CustomEditor(typeof(CustomLifetimeScope), true)]
	internal sealed class CustomLifetimeScopeEditor : Editor
	{
		private SerializedProperty _injectableComponents;

		private CustomLifetimeScope CustomLifetimeScope => ((CustomLifetimeScope)target);

		private void OnEnable()
		{
			_injectableComponents = serializedObject.FindProperty(nameof(_injectableComponents));
		}

		public override void OnInspectorGUI()
		{
			base.OnInspectorGUI();

			serializedObject.UpdateIfRequiredOrScript();

			if (GUILayout.Button(nameof(FindAllInjectedBehaviours).SplitUpperCase()))
			{
				FindAllInjectedBehaviours();
			}

			serializedObject.ApplyModifiedProperties();
		}

		private void FindAllInjectedBehaviours()
		{
			var injectableComponents = FindObjectsByType<Component>(FindObjectsInactive.Include, FindObjectsSortMode.None).Where(component => (component is IInjectable)).ToArray();

			var isPrefab = (PrefabUtility.IsPartOfAnyPrefab(target) && (!PrefabUtility.IsPartOfPrefabInstance(target)));

			if (EditorSceneManager.IsPreviewScene(CustomLifetimeScope.gameObject.scene) || isPrefab)
			{
				var rootTransform = (CustomLifetimeScope.transform.root ?? CustomLifetimeScope.transform);

				injectableComponents = rootTransform.GetComponentsInChildren<Component>(true).Where(component => (component is IInjectable)).ToArray();
			}

			_injectableComponents.FillArray(injectableComponents);
		}
	}
}