using Cysharp.Threading.Tasks;
using Runtime.Data.Configs;
using Runtime.Data.Configs.Core;
using Runtime.Data.Progress;
using Runtime.Infrastructure.Core;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Runtime.Infrastructure.Services.SaveProgressServices
{
	internal sealed class DataService : IInitializationInformer, IInitializable, IDisposable
	{
		private const string SAVE_KEY = "user_data";

		private IProgressConfig _progressConfig;

		private IUserInfo _userInfo;

		private Action _initialized;

		private Boolean _isInitialized;

		internal IUserInfo UserInfo => _userInfo;

		Boolean IInitializationInformer.IsInitialized => _isInitialized;

		event Action IInitializationInformer.Initialized
		{
			add => _initialized += value;
			remove => _initialized -= value;
		}

		[Inject]
		private void Construct(IProgressConfig progressConfig)
		{
			_progressConfig = progressConfig;
		}

		async void IInitializable.Initialize()
		{
			await LoadData();

			_isInitialized = true;
			_initialized?.Invoke();
		}

		void IDisposable.Dispose()
		{
			SaveData();
		}

		internal void SaveData()
		{
			try
			{
				var jsonData = JsonUtility.ToJson(_userInfo);
				PlayerPrefs.SetString(SAVE_KEY, jsonData);
				PlayerPrefs.Save();
				Debug.Log("Данные успешно сохранены");
			}
			catch (Exception e)
			{
				Debug.LogError($"Ошибка при сохранении данных: {e.Message}");
			}
		}

		private async UniTask LoadData()
		{
			try
			{
				if (PlayerPrefs.HasKey(SAVE_KEY))
				{
					var jsonData = PlayerPrefs.GetString(SAVE_KEY);
					var userInfo = JsonUtility.FromJson<UserInfo>(jsonData);
					Debug.Log("Данные успешно загружены");
					_userInfo = userInfo;

					return;
				}

				Debug.Log("Сохранённые данные не найдены, создаём новые");
				_userInfo = CreateNewUserInfo();
			}
			catch (Exception e)
			{
				Debug.LogError($"Ошибка при загрузке данных: {e.Message}");
				_userInfo = CreateNewUserInfo();
			}
		}

		private UserInfo CreateNewUserInfo()
		{
			var userInfo = _progressConfig.UserInfo.Clone();
			return userInfo;
		}
	}
}