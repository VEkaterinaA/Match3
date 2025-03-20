using Runtime.Data.Configs.Core;
using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.SaveProgressServices.Core;
using System;
using UnityEngine;
using VContainer;
using VContainer.Unity;
using Time = UnityEngine.Time;

namespace Runtime.Infrastructure.Services.App.Core
{
	internal abstract class ApplicationService : IApplicationService, IInitializable, IDisposable
	{
		private IPersistentProgressService _persistentProgressService;
		private ILoopsService _loopsService;
		private IGameConfig _gameConfig;

		private Single _secondsAfterLastSave;

		[Inject]
		internal void Construct(IPersistentProgressService persistentProgressService, IGameConfig gameConfig, ILoopsService loopsService)
		{
			_persistentProgressService = persistentProgressService;
			_loopsService = loopsService;
			_gameConfig = gameConfig;
		}

		void IInitializable.Initialize()
		{
			_loopsService.Updated += HandleUpdate;

			Application.targetFrameRate = 60;
		}

		void IDisposable.Dispose()
		{
			_loopsService.Updated -= HandleUpdate;
		}

		void IApplicationService.RequestQuit()
		{
			//_persistentProgressService.SaveActiveProgress();

			RequestQuit();
		}

		protected abstract void RequestQuit();

		private void HandleUpdate()
		{
			_secondsAfterLastSave += Time.deltaTime;

			if ((_secondsAfterLastSave > _gameConfig.SaveCooldown) && (_persistentProgressService.ActiveProgress is not null))
			{
				//_persistentProgressService.SaveActiveProgress();
				_secondsAfterLastSave = 0.0F;
			}
		}
	}
}