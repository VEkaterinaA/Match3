using Runtime.Data.Constants.Strings;
using Runtime.Data.Progress;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace HiddenLightEditor.MenuItems
{
	internal static class Tools
	{
		[MenuItem("Tools/Clear Data")]
		public static void DeleteProgress()
		{
			if (Load<UserInfo>(DataPath.UserInfo) is IUserInfo userInfo)
			{
				File.Delete(DataPath.UserInfo);

				foreach (var progressSlotID in userInfo.ProgressSlotsIDs)
				{
					File.Delete(DataPath.GetForPlayerProgressWithID(progressSlotID));
				}
			}

			Debug.Log($"[{nameof(Tools)}] All data has been cleared.");
		}


		[MenuItem("Tools/Fix Sprites")]
		public static void FixSprites()
		{
			var spriteRenderers = UnityEngine.Object.FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);

			var spriteRenderersToFix = new List<SpriteRenderer>(spriteRenderers.Length);
			var scene = spriteRenderers.First().gameObject.scene;

			foreach (var spriteRenderer in spriteRenderers)
			{
				if (spriteRenderer.maskInteraction is SpriteMaskInteraction.None)
				{
					spriteRenderer.maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
					spriteRenderersToFix.Add(spriteRenderer);
				}
			}

			if (!Application.isPlaying)
			{
				EditorSceneManager.MarkSceneDirty(scene);
				EditorSceneManager.SaveScene(scene);
			}

			foreach (var spriteRenderer in spriteRenderersToFix)
			{
				spriteRenderer.maskInteraction = SpriteMaskInteraction.None;
			}

			if (!Application.isPlaying)
			{
				EditorSceneManager.MarkSceneDirty(scene);
				EditorSceneManager.SaveScene(scene);
			}

			Debug.Log($"[{nameof(Tools)}] Fixed {spriteRenderersToFix.Count} sprites");
		}


		private static TObject Load<TObject>(String fullPath) where TObject : class
		{
			if (File.Exists(fullPath))
			{
				try
				{
					using var fileStream = new FileStream(fullPath, FileMode.Open);
					using var streamReader = new StreamReader(fileStream);

					var data = streamReader.ReadToEnd();

					return JsonUtility.FromJson<TObject>(data);
				}
				catch (Exception exception)
				{
					Debug.LogError($"[Load] Error occured when trying to load data from file {fullPath}\n{exception}");
				}
			}

			return null;
		}
	}
}