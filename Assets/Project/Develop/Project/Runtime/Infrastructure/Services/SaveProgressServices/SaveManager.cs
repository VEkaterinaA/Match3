using Cysharp.Threading.Tasks;
using Runtime.Infrastructure.Core;
using Runtime.Infrastructure.Factories;
using Runtime.Infrastructure.Factories.Core;
using System;
using UnityEngine;
using UnityEngine.Localization.Settings;
using VContainer;
using VContainer.Unity;

namespace Runtime.Data.Progress
{
	namespace Runtime.Infrastructure.Services.SaveProgressServices
	{
		internal sealed class SaveManager : IInitializationInformer, IInitializable
		{
			private const string SAVE_KEY = "user_data";


			private IDataFactory _progressFactory;
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
			private void Construct(IDataFactory progressFactory)
			{
				_progressFactory = progressFactory;
				_userInfo = _progressFactory.CreateUserInfo();
			}

			async void IInitializable.Initialize()
			{
				await LoadData();

				_isInitialized = true;
				_initialized?.Invoke();
			}
			internal void SaveData(UserInfo userInfo)
			{
				try
				{
					var jsonData = JsonUtility.ToJson(userInfo);
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
				var userInfo = new UserInfo();
				return userInfo;
			}
		}
	}
}