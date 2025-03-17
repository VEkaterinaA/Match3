using Runtime.Extensions.System;
using Runtime.Infrastructure.Services.Localization.Core;
using Runtime.Visual.UI.VisualElements.TemplateWrappers.Arrows.Core;
using System;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VContainer;

namespace Runtime.Visual.UI.VisualElements.TemplateWrappers.Arrows
{
	internal sealed class SubtitlesArrowsButtons : ArrowsButtons
	{
		private ILocalizationService _localizationService;

		private Locale SubtitlesLocale => _localizationService.SubtitlesLocale;

		internal SubtitlesArrowsButtons(VisualElement rootVisualElement) : base(rootVisualElement)
		{
		}

		[Inject]
		internal void Construct(ILocalizationService localizationService)
		{
			_localizationService = localizationService;

			Subscribe();

			localizationService.InvokeAfterInitialization(UpdateView);
		}

		protected override void HandleRightButtonClick()
		{
			_localizationService.SubtitlesLocaleIndex++;

			UpdateView();
		}

		protected override void HandleLeftButtonClick()
		{
			_localizationService.SubtitlesLocaleIndex--;

			UpdateView();
		}

		protected override void Subscribe()
		{
			base.Subscribe();

			_localizationService.LocaleChanged += UpdateView;
		}

		protected override void Unsubscribe()
		{
			base.Unsubscribe();

			_localizationService.LocaleChanged -= UpdateView;
		}

		private void UpdateView()
		{
			_localizationService.GetLocalizedString(_localizationService.LanguageLocale, SubtitlesLocale.LocaleName, (localizedText => Label.text = localizedText));
		}
	}
}