using Runtime.Visual.UI.UIDocumentWrappers.Popups.Core;
using System;
using UnityEngine.UIElements;

namespace Runtime.Visual.UI.UIDocumentWrappers.Popups
{
	internal sealed class ConfirmationPopup : Popup
	{
		private Action _cancelAction;
		private Action _applyAction;

		private Label DescriptionLabel { get; }

		private Button CancelButton { get; }

		private Button ApplyButton { get; }

		internal ConfirmationPopup(UIDocument uiDocument, Boolean canOverlapOtherPopups) : base(uiDocument, canOverlapOtherPopups)
		{
			DescriptionLabel = RootVisualElement.Q<Label>(nameof(DescriptionLabel));
			CancelButton = RootVisualElement.Q<Button>(nameof(CancelButton));
			ApplyButton = RootVisualElement.Q<Button>(nameof(ApplyButton));
		}

		internal void Setup(String descriptionLabelLocalizationKey, String cancelButtonLocalizationKey, String applyButtonLocalizationKey, Action cancelAction, Action applyAction)
		{
			LocalizationService.GetLocalizedString(LocalizationService.LanguageLocale, descriptionLabelLocalizationKey, (localizedText => DescriptionLabel.text = localizedText));
			LocalizationService.GetLocalizedString(LocalizationService.LanguageLocale, cancelButtonLocalizationKey, (localizedText => CancelButton.text = localizedText));
			LocalizationService.GetLocalizedString(LocalizationService.LanguageLocale, applyButtonLocalizationKey, (localizedText => ApplyButton.text = localizedText));

			_cancelAction = cancelAction;
			_applyAction = applyAction;
		}

		protected override void Hide()
		{
			base.Hide();

			_cancelAction = null;
			_applyAction = null;
		}

		protected override void Subscribe()
		{
			base.Subscribe();

			CancelButton.clicked += HandleCancelButtonClick;
			ApplyButton.clicked += HandleApplyButtonClick;
		}

		protected override void Unsubscribe()
		{
			base.Unsubscribe();

			CancelButton.clicked -= HandleCancelButtonClick;
			ApplyButton.clicked -= HandleApplyButtonClick;
		}

		private void HandleCancelButtonClick()
		{
			_cancelAction?.Invoke();

			PopupsService.Hide<ConfirmationPopup>();
		}

		private void HandleApplyButtonClick()
		{
			_applyAction?.Invoke();

			PopupsService.Hide<ConfirmationPopup>();
		}
	}
}