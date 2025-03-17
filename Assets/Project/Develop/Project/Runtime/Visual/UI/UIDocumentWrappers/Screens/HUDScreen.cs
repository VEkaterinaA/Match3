using Runtime.Extensions.System;
using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.Localization.Core;
using Runtime.Infrastructure.Services.SaveProgressServices.Core;
using Runtime.Infrastructure.Services.UIServices.Core;
using System;
using UnityEngine.UIElements;
using VContainer;
using Screen = Runtime.Visual.UI.UIDocumentWrappers.Screens.Core.Screen;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens
{
	internal class HUDScreen : Screen
	{
		private IPersistentProgressService _persistentProgressService;
		private ILocalizationService _localizationService;
		private IScreensService _screensService;
		private ILoopsService _loopsService;

		internal event Action OnHintButtonPressed;

		internal Boolean IsActiveHandle;

		internal HUDScreen(UIDocument uiDocument) : base(uiDocument)
		{

		}

		[Inject]
		internal void Construct(IPersistentProgressService persistentProgressService, IScreensService screensService, ILocalizationService localizationService,
								ILoopsService loopsService)
		{
			_persistentProgressService = persistentProgressService;
			_localizationService = localizationService;
			_screensService = screensService;
			_loopsService = loopsService;

			_persistentProgressService.InvokeAfterInitialization(Load);
			_persistentProgressService.ActiveProgressChanged += Load;
		}

		private void Load()
		{

		}

	}
}