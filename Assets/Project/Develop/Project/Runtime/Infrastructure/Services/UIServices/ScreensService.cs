using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.Extensions.System;
using Runtime.Infrastructure.Core;
using Runtime.Infrastructure.Factories.Core;
using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Screens;
using Runtime.Visual.UI.UIDocumentWrappers.Screens.Core;
using System;
using System.Collections.Generic;
using UnityEngine;
using VContainer;
using Screen = UnityEngine.Screen;

namespace Runtime.Infrastructure.Services.UIServices
{
	internal sealed class ScreensService : IInitializationInformer, IScreensService
	{
		private IUIDocumentsFactory<ScreenType> _uiDocumentsFactory;

		private readonly Dictionary<Type, IScreen> _screens = new Dictionary<Type, IScreen>();
		private readonly Transform _screensParentTransform;
		private readonly LoadingScreen _loadingScreen;

		private Action _initialized;

		private Boolean _isInitialized;

		event Action IInitializationInformer.Initialized
		{
			add => _initialized += value;
			remove => _initialized -= value;
		}

		Boolean IInitializationInformer.IsInitialized => _isInitialized;

		private IScreensService Service => this;

		internal ScreensService(LoadingScreen loadingScreen, Transform screensParentTransform)
		{
			_screensParentTransform = screensParentTransform;
			_loadingScreen = loadingScreen;

			((IScreen) loadingScreen).Initialize();
		}

		[Inject]
		internal void Construct(IUIDocumentsFactory<ScreenType> uiDocumentsFactory)
		{
			_uiDocumentsFactory = uiDocumentsFactory;

			uiDocumentsFactory.InvokeAfterInitialization(Initialize);
		}

		private void Initialize()
		{
			foreach (var screen in _uiDocumentsFactory.CreateAll(_screensParentTransform))
			{
				_screens.Add(screen.GetType(), (IScreen) screen);

				((IScreen) screen).Initialize();
			}

			_isInitialized = true;
			_initialized?.Invoke();
		}

		Vector2 IScreensService.GetScreenCenter()
		{
			Single screenWidth = Screen.width;
			Single screenHeight = Screen.height;

			var screenCenter = new Vector2(screenWidth / 2, screenHeight / 2);

			return screenCenter;
		}

		Vector2 IScreensService.GetScreenSize()
		{
			Single screenWidth = Screen.width;
			Single screenHeight = Screen.height;

			return new Vector2(screenWidth, screenHeight);
		}

		void IScreensService.Add(IScreen screen)
		{
			_screens.Add(screen.GetType(), screen);

			screen.Initialize();
		}

		void IScreensService.Show<TScreen>(Action completionAction)
		{
			var screen = Service.Get<TScreen>();

			if (screen.IsShowed)
			{
				completionAction?.Invoke();
			}
			else
			{
				screen.Show(completionAction);
			}
		}

		void IScreensService.ShowInstantly(Type screenType)
		{
			Service.Get(screenType).ShowInstantly();
		}

		void IScreensService.Hide<TScreen>(Action completionAction)
		{
			var screen = Service.Get<TScreen>();

			if (screen.IsShowed)
			{
				screen.Hide(completionAction);
			}
			else
			{
				completionAction?.Invoke();
			}
		}

		void IScreensService.Hide<TScreen>()
		{
			Service.Hide<TScreen>(null);
		}

		TScreen IScreensService.Get<TScreen>()
		{
			if (typeof(TScreen) == typeof(LoadingScreen))
			{
				return (_loadingScreen as TScreen);
			}

			return (_screens[typeof(TScreen)] as TScreen);
		}

		IScreen IScreensService.Get(Type screenType)
		{
			if (screenType == typeof(LoadingScreen))
			{
				return _loadingScreen;
			}

			return _screens[screenType];
		}
	}

}
