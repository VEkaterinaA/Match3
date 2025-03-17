using Cysharp.Threading.Tasks;
using Runtime.Data.Constants.Strings;
using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.SaveProgressServices.Core;
using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Popups.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Screens;
using System;
using System.Collections.Generic;
using UnityEngine.UIElements;
using VContainer;

namespace Runtime.Visual.UI.UIDocumentWrappers.Popups
{
	internal sealed class SaveProgressPopup : Popup
	{
		private IPersistentProgressService _persistentProgressService;
		private IScreensService _screensService;
		private ILoopsService _loopsService;

		private readonly List<VisualElement> _manualRepaintVisualElements;

		private VisualElement ScreenshotVisualElement { get; }

		private TextField SaveNameTextField { get; }

		private Label DateTimeLabel { get; }

		private Button SaveButton { get; }

		internal SaveProgressPopup(UIDocument uiDocument, Boolean canOverlapOtherPopups) : base(uiDocument, canOverlapOtherPopups)
		{
			ScreenshotVisualElement = RootVisualElement.Q<VisualElement>(nameof(ScreenshotVisualElement));
			SaveNameTextField = RootVisualElement.Q<TextField>(nameof(SaveNameTextField));
			DateTimeLabel = RootVisualElement.Q<Label>(nameof(DateTimeLabel));
			SaveButton = RootVisualElement.Q<Button>(nameof(SaveButton));

			_manualRepaintVisualElements = RootVisualElement.Query<VisualElement>(null, ClassName.ManualRepaint).ToList();
		}

		[Inject]
		internal void Construct(IPersistentProgressService persistentProgressService, ILoopsService loopsService, IScreensService screensService)
		{
			_persistentProgressService = persistentProgressService;
			_screensService = screensService;
			_loopsService = loopsService;
		}

		protected override async void Show()
		{
			base.Show();

			ScreenshotVisualElement.style.backgroundImage = _persistentProgressService.ReadOnlyProgress.ScreenshotTexture2D;
			SaveNameTextField.value = _persistentProgressService.ReadOnlyProgress.Name;

			await UniTask.NextFrame();

			foreach (var manualRepaintVisualElement in _manualRepaintVisualElements)
			{
				manualRepaintVisualElement.MarkDirtyRepaint();
			}
		}

		protected override void Subscribe()
		{
			base.Subscribe();

			SaveButton.clicked += HandleSaveButtonClick;
			_loopsService.Updated += HandleUpdate;
		}

		protected override void Unsubscribe()
		{
			base.Unsubscribe();

			SaveButton.clicked -= HandleSaveButtonClick;
			_loopsService.Updated -= HandleUpdate;
		}

		private async void HandleSaveButtonClick()
		{
			_persistentProgressService.ActiveProgress = await _persistentProgressService.CreateProgressSlot(SaveNameTextField.text, _persistentProgressService.ActiveProgress);

			PopupsService.Hide<SaveProgressPopup>();
			_screensService.Get<MainMenuScreen>().PlaySaveAnimation();
		}

		private void HandleUpdate()
		{
			DateTimeLabel.text = $"{DateTime.Now:d} {DateTime.Now:T}";
		}
	}
}