using Runtime.Data.Progress;
using Runtime.Extensions.LitMotion;
using Runtime.Extensions.System;
using Runtime.Extensions.UnityEngine.UIElements;
using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.Localization.Core;
using Runtime.Infrastructure.Services.SaveProgressServices.Core;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using Screen = Runtime.Visual.UI.UIDocumentWrappers.Screens.Core.Screen;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens
{
	internal sealed class GameScreen : Screen
	{
		private IPersistentProgressService _persistentProgressService;
		private ILocalizationService _localizationService;
		private ICamerasService _camerasService;
		private ILoopsService _loopsService;

		private String _activeSubtitlesID;

		internal GameScreen(UIDocument uiDocument) : base(uiDocument)
		{
		}

		[Inject]
		internal void Construct(IPersistentProgressService persistentProgressService, ILocalizationService localizationService, ICamerasService camerasService,
			ILoopsService loopsService)
		{
			_persistentProgressService = persistentProgressService;
			_localizationService = localizationService;
			_camerasService = camerasService;
			_loopsService = loopsService;
		}

		protected override void UpdateLocalization()
		{
			base.UpdateLocalization();

/*			if (!_activeSubtitlesID.IsNullOrEmpty())
			{
				_localizationService.GetLocalizedString(_localizationService.SubtitlesLocale, _activeSubtitlesID, (localizedText => SubtitlesLabel.text = localizedText));
			}*/
		}
		

		protected override void Subscribe()
		{
			base.Subscribe();

			_localizationService.LocaleChanged += UpdateLocalization;
		}

		protected override void Unsubscribe()
		{
			base.Unsubscribe();

			_localizationService.LocaleChanged -= UpdateLocalization;
		}
		
	}
}