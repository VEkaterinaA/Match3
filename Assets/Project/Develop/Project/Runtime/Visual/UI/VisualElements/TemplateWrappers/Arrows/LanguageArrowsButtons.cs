using Runtime.Extensions.System;
using Runtime.Infrastructure.Services.Localization.Core;
using Runtime.Visual.UI.VisualElements.TemplateWrappers.Arrows.Core;
using System;
using UnityEngine.Localization;
using UnityEngine.UIElements;
using VContainer;

namespace Runtime.Visual.UI.VisualElements.TemplateWrappers.Arrows
{
	internal sealed class LanguageArrowsButtons : ArrowsButtons
	{
		private ILocalizationService _localizationService;

		private Locale LanguageLocale => _localizationService.LanguageLocale;

		internal LanguageArrowsButtons(VisualElement rootVisualElement) : base(rootVisualElement)
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
			_localizationService.SelectedLocaleIndex++;

			UpdateView();
		}

		protected override void HandleLeftButtonClick()
		{
			_localizationService.SelectedLocaleIndex--;

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
			_localizationService.GetLocalizedString(LanguageLocale, LanguageLocale.LocaleName, (localizedText => Label.text = localizedText));
		}
	}
}