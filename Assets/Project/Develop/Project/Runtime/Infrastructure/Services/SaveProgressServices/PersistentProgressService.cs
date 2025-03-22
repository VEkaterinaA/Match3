using Cysharp.Threading.Tasks;
using Runtime.Data.Configs.Core;
using Runtime.Data.Constants.Strings;
using Runtime.Data.Progress;
using Runtime.Extensions.System;
using Runtime.Infrastructure.Core;
using Runtime.Infrastructure.Factories.Core;
using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.SaveProgressServices.Core;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Action = System.Action;

namespace Runtime.Infrastructure.Services.SaveProgressServices
{
	internal sealed class PersistentProgressService : IPersistentProgressService, IInitializationInformer, IInitializable
	{
		private IFileSystemService _fileSystemService;
		private IPersistentProgress _activeProgress;
		private IProgressFactory _progressFactory;
		private IProgressConfig _progressConfig;
		private IRandomService _randomService;
		private IUserInfo _userInfo;

		private readonly Dictionary<String, IPersistentProgress> _progressSlots = new Dictionary<String, IPersistentProgress>(4);

		private Action<IPersistentProgress> _progressCreated;
		private Action _activeProgressChanged;
		private Action _progressSavingStarted;
		private Action _initialized;

		private Boolean _isInitialized;
		private Boolean _isAllowSaveProgress;

		event Action<IPersistentProgress> IPersistentProgressService.ProgressCreated
		{
			add => _progressCreated += value;
			remove => _progressCreated -= value;
		}

		event Action IPersistentProgressService.ActiveProgressChanged
		{
			add => _activeProgressChanged += value;
			remove => _activeProgressChanged -= value;
		}

		event Action IPersistentProgressService.ProgressSavingStarted
		{
			add => _progressSavingStarted += value;
			remove => _progressSavingStarted -= value;
		}

		event Action IInitializationInformer.Initialized
		{
			add => _initialized += value;
			remove => _initialized -= value;
		}

		IReadOnlyDictionary<String, IPersistentProgress> IPersistentProgressService.ProgressSlots => _progressSlots;

		IReadOnlyProgress IPersistentProgressService.ReadOnlyProgress => (_activeProgress ?? _progressConfig.ProgressPrototype);

		Boolean IInitializationInformer.IsInitialized => _isInitialized;

		IUserInfo IPersistentProgressService.UserInfo => _userInfo;

		private IPersistentProgressService Service => this;

		IPersistentProgress IPersistentProgressService.ActiveProgress
		{
			get => _activeProgress;
			set
			{
				_activeProgress = value;

				_isAllowSaveProgress = true;

				_userInfo.ActiveProgressID = ((value is null) ? null : _progressSlots.First(pair => (pair.Value == _activeProgress)).Key);

				_progressConfig.Display(value, _userInfo);

				_activeProgressChanged?.Invoke();
			}
		}

		Boolean IPersistentProgressService.IsAllowSaveProgress
		{
			get => _isAllowSaveProgress;
			set { _isAllowSaveProgress = value; Debug.Log(_isAllowSaveProgress); }
		}

		[Inject]
		internal void Construct(IFileSystemService fileSystemService, IProgressConfig progressConfig, IProgressFactory progressFactory, IRandomService randomService)
		{
			_fileSystemService = fileSystemService;
			_progressFactory = progressFactory;
			_progressConfig = progressConfig;
			_randomService = randomService;
			_userInfo = _progressFactory.CreateUserInfo();
		}

		async void IInitializable.Initialize()
		{
			await LoadGameData();

			_isInitialized = true;
			_initialized?.Invoke();
		}

		private async UniTask LoadGameData()
		{
			try
			{
				if (await _fileSystemService.ExistsAsync(DataPath.UserInfo))
				{
					var userInfoJson = await _fileSystemService.ReadAllTextAsync(DataPath.UserInfo);
					_userInfo = JsonUtility.FromJson<UserInfo>(userInfoJson);
				}
				else
				{
					_userInfo = _progressFactory.CreateUserInfo();
				}

				foreach (var progressSlotID in _userInfo.ProgressSlotsIDs)
				{
					var progressPath = DataPath.GetForPlayerProgressWithID(progressSlotID);
					if (await _fileSystemService.ExistsAsync(progressPath))
					{
						var progressJson = await _fileSystemService.ReadAllTextAsync(progressPath);
						var progress = JsonUtility.FromJson<PersistentProgress>(progressJson);
						_progressSlots.Add(progressSlotID, progress);
					}
				}

				if (_userInfo.ActiveProgressID != null && _progressSlots.TryGetValue(_userInfo.ActiveProgressID, out var activeProgress))
				{
					Service.ActiveProgress = activeProgress;
				}
			}
			catch (Exception e)
			{
				Debug.LogError($"Error loading game data: {e}");
				_userInfo = _progressFactory.CreateUserInfo();
			}
		}

		UniTask IPersistentProgressService.SaveGameData()
		{
			return SaveGameData();
		}

		private async UniTask SaveGameData()
		{
			try
			{
				_progressSavingStarted?.Invoke();

				var userInfoJson = JsonUtility.ToJson(_userInfo);
				await _fileSystemService.WriteAllTextAsync(DataPath.UserInfo, userInfoJson);

				foreach (var progressSlot in _progressSlots)
				{
					var progressPath = DataPath.GetForPlayerProgressWithID(progressSlot.Key);
					var progressJson = JsonUtility.ToJson(progressSlot.Value);
					await _fileSystemService.WriteAllTextAsync(progressPath, progressJson);
				}
			}
			catch (Exception e)
			{
				Debug.LogError($"Error saving game data: {e}");
			}
		}
	}
}
