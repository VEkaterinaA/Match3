using Cysharp.Threading.Tasks;
using Runtime.Data.Constants.Enums;
using Runtime.Infrastructure.Services.Core;
using System;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Runtime.Infrastructure.Services
{
	internal sealed class SceneLoadService : ISceneLoadService, IDisposable
	{
		private Action _activeSceneChanged;

		event Action ISceneLoadService.ActiveSceneChanged
		{
			add => _activeSceneChanged += value;
			remove => _activeSceneChanged -= value;
		}

		internal SceneLoadService()
		{
			SceneManager.activeSceneChanged += HandleActiveSceneChange;
		}

		void IDisposable.Dispose()
		{
			SceneManager.activeSceneChanged -= HandleActiveSceneChange;
		}

		AsyncOperation ISceneLoadService.Load(SceneName sceneName, LoadSceneMode loadSceneMode)
		{
			return SceneManager.LoadSceneAsync(sceneName.ToString(), loadSceneMode);
		}
		AsyncOperationHandle ISceneLoadService.LoadSceneAsync(SceneName sceneName)
		{
			return Addressables.LoadSceneAsync(sceneName.ToString());
		}

		private async UniTaskVoid LoadSceneAsync(string sceneKey)
		{
			var handler = Addressables.LoadSceneAsync(sceneKey);
			await UniTask.WaitUntil(() => handler.IsDone);
		}

		private void HandleActiveSceneChange(Scene previousScene, Scene currentScene)
		{
			_activeSceneChanged?.Invoke();
		}
	}

}
