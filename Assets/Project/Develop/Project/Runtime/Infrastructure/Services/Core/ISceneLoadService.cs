using Runtime.Data.Constants.Enums;
using System;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.SceneManagement;

namespace Runtime.Infrastructure.Services.Core
{
	internal interface ISceneLoadService
	{
		internal event Action ActiveSceneChanged;

		internal AsyncOperation Load(SceneName sceneName, LoadSceneMode loadSceneMode);

		internal AsyncOperationHandle LoadSceneAsync(SceneName sceneName);
	}
}
