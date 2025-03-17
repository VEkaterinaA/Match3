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
		private ICamerasService _camerasService;
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
		internal void Construct(IFileSystemService fileSystemService, IProgressConfig progressConfig, IProgressFactory progressFactory, ICamerasService camerasService, IRandomService randomService)
		{
			_fileSystemService = fileSystemService;
			_progressFactory = progressFactory;
			_progressConfig = progressConfig;
			_camerasService = camerasService;
			_randomService = randomService;
		}

		async void IInitializable.Initialize()
		{
			await LoadGameData();

			_isInitialized = true;
			_initialized?.Invoke();
		}

		async UniTask IPersistentProgressService.LoadingSavedProgress(String progressName, IPersistentProgress persistentProgress)
		{
			LevelInfo lastActiveLevel = null;

			if (_activeProgress != null)
			{
				Service.DeleteProgressSlot(Service.ActiveProgress);
				lastActiveLevel = Service.ActiveProgress.SavedLevels.ActiveLevelInfo;
			}

			Service.ActiveProgress = await Service.CreateProgressSlot(progressName, persistentProgress);

			if (lastActiveLevel != null)
			{
				if (lastActiveLevel != Service.ActiveProgress.SavedLevels.ActiveLevelInfo)
				{
					//load scene
				}
			}

			//LoadGameData();
		}

		async UniTask IPersistentProgressService.LoadingActiveProgress()
		{
			_progressSlots.Clear();
			await LoadGameData();
		}

		private async UniTask LoadGameData()
		{
			_userInfo = _fileSystemService.LoadFromJSON<UserInfo>(DataPath.UserInfo);

			if (_userInfo is null)
			{
				_userInfo = _progressFactory.CreateUserInfo();
				_fileSystemService.Save(_userInfo.Serialize(), DataPath.UserInfo);
			}

			for (var progressSlotIndex = 0; (progressSlotIndex < _userInfo.ProgressSlotsIDs.Count); progressSlotIndex++)
			{
				var progressSlotID = _userInfo.ProgressSlotsIDs[progressSlotIndex];
				var playerProgress = _fileSystemService.LoadFromJSON<PersistentProgress>(DataPath.GetForPlayerProgressWithID(progressSlotID), _userInfo.ID);

				if (playerProgress is null)
				{
					_userInfo.ProgressSlotsIDs.RemoveAt(progressSlotIndex);
					progressSlotIndex--;
				}
				else
				{
					((IPersistentProgress) playerProgress).ScreenshotTexture2D = _fileSystemService.LoadTexture2D(Path.Combine(Application.persistentDataPath, $"{progressSlotID}.icon"));
					_progressSlots.Add(progressSlotID, playerProgress);

					if (progressSlotID == _userInfo.ActiveProgressID)
					{
						Service.ActiveProgress = playerProgress;
					}
				}
			}
		}
		async UniTask<IPersistentProgress> IPersistentProgressService.CreateProgressSlot(String progressName, IPersistentProgress persistentProgress)
		{
			var playerProgress = (persistentProgress is null ? _progressFactory.CreatePlayerProgress() : persistentProgress.Clone());
			playerProgress.Name = progressName;

			var progressSlotID = _randomService.CreateGUID(_userInfo.ProgressSlotsIDs.ToArray());

			_camerasService.TakeScreenshot(texture2D =>
			{
				playerProgress.ScreenshotTexture2D = texture2D;
				_fileSystemService.Save(texture2D.GetRawTextureData(), Path.Combine(Application.persistentDataPath, $"{progressSlotID}.icon"));
			});

			_progressSlots.Add(progressSlotID, playerProgress);

			playerProgress.SavedDateTime = DateTime.Now;
			_fileSystemService.Save(playerProgress.Serialize().Encrypt(_userInfo.ID), DataPath.GetForPlayerProgressWithID(progressSlotID));

			_userInfo.ProgressSlotsIDs.Add(progressSlotID);
			_fileSystemService.Save(_userInfo.Serialize(), DataPath.UserInfo);

			_progressCreated?.Invoke(playerProgress);

			return playerProgress;
		}

		void IPersistentProgressService.DeleteProgressSlot(IPersistentProgress persistentProgress)
		{
			Service.DeleteProgressSlot(_progressSlots.First(pair => (pair.Value == persistentProgress)).Key);
		}

		void IPersistentProgressService.DeleteProgressSlot(String slotID)
		{
			_progressSlots.Remove(slotID);

			_fileSystemService.Delete(DataPath.GetForPlayerProgressWithID(slotID));
			_fileSystemService.Delete(Path.Combine(Application.persistentDataPath, $"{slotID}.icon"));
		}

		async UniTask IPersistentProgressService.SaveActiveProgressAsync()
		{
			_progressSavingStarted?.Invoke();

			_activeProgress.SavedDateTime = DateTime.Now;
			await _fileSystemService.SaveAsync(_activeProgress.Serialize().Encrypt(_userInfo.ID), DataPath.GetForPlayerProgressWithID(_userInfo.ActiveProgressID));

			await _fileSystemService.SaveAsync(_userInfo.Serialize(), DataPath.UserInfo);
		}

		void IPersistentProgressService.SaveActiveProgress()
		{
			if (!_isAllowSaveProgress)
			{
				return;
			}
			_progressSavingStarted?.Invoke();

			_activeProgress.SavedDateTime = DateTime.Now;
			_fileSystemService.Save(_activeProgress.Serialize().Encrypt(_userInfo.ID), DataPath.GetForPlayerProgressWithID(_userInfo.ActiveProgressID));

			_fileSystemService.Save(_userInfo.Serialize(), DataPath.UserInfo);
		}
	}
}
