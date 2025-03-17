using Runtime.Data.Configs.Core;
using Runtime.Data.Constants.Enums.AssetReferencesTypes;
using Runtime.Data.Progress;
using Runtime.Extensions.System;
using Runtime.Extensions.UnityEngine.UIElements;
using Runtime.Infrastructure.GameStateMachine.Core;
using Runtime.Infrastructure.GameStateMachine.States;
using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.SaveProgressServices.Core;
using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Popups.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Screens;
using Runtime.Visual.UI.VisualElements.TemplateWrappers;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using VContainer;

namespace Runtime.Visual.UI.UIDocumentWrappers.Popups
{
	internal sealed class LoadGamePopup : Popup, IDisposable
	{
/*		private IPersistentProgressService _persistentProgressService;
		private IGameStateMachine _gameStateMachine;
		private ITemplatesFactory _templatesFactory;
		private IScreensService _screensService;
		private IGameService _gameService;
		private IGameConfig _gameConfig;

		private List<ProgressSlot> _progressSlots;

		private readonly CompositeMotionHandle _bottomButtonsCompositeMotionHandle = new CompositeMotionHandle(3);
		private ProgressSlot _selectedProgressSlot;

		private const Single _bottomButtonsFadeDuration = 0.2F;
		private Boolean _bottomButtonsAreEnabled = true;

		private ScrollView ProgressSlotsScrollView { get; }

		private Button DeleteButton { get; }

		private Button RenameButton { get; }

		private Button LoadButton { get; }

		private Boolean BottomButtonsAreEnabled
		{
			get => _bottomButtonsAreEnabled;
			set
			{
				if (_bottomButtonsAreEnabled == value)
				{
					return;
				}

				_bottomButtonsAreEnabled = value;

				UpdateButtonsView(_bottomButtonsAreEnabled, DeleteButton, RenameButton, LoadButton);

				return;

				void UpdateButtonsView(Boolean areEnabled, params Button[] buttons)
				{
					_bottomButtonsCompositeMotionHandle.Cancel();

					if (areEnabled)
					{
						foreach (var button in buttons)
						{
							button.style.opacity = 1.0F;
							button.SetEnabled(true);
						}
					}
					else
					{
						foreach (var button in buttons)
						{
							button.style.CreateMotion(1.0F, 0.0F, _bottomButtonsFadeDuration).BindToOpacity().AddTo(_bottomButtonsCompositeMotionHandle);
							button.SetEnabled(true);
						}
					}
				}
			}
		}*/

		internal LoadGamePopup(UIDocument uiDocument, Boolean canOverlapOtherPopups) : base(uiDocument, canOverlapOtherPopups)
		{
/*			ProgressSlotsScrollView = RootVisualElement.Q<ScrollView>(nameof(ProgressSlotsScrollView));
			DeleteButton = RootVisualElement.Q<Button>(nameof(DeleteButton));
			RenameButton = RootVisualElement.Q<Button>(nameof(RenameButton));
			LoadButton = RootVisualElement.Q<Button>(nameof(LoadButton));*/
		}
/*
		[Inject]
		internal void Construct(IPersistentProgressService persistentProgressService, IGameStateMachine gameStateMachine, IGameService gameService, ITemplatesFactory templatesFactory, IGameConfig gameConfig,
								IScreensService screensService)
		{
			_persistentProgressService = persistentProgressService;
			_gameStateMachine = gameStateMachine;
			_templatesFactory = templatesFactory;
			_screensService = screensService;
			_gameService = gameService;
			_gameConfig = gameConfig;

			templatesFactory.InvokeAfterInitialization(Initialize);
		}*/

		void IDisposable.Dispose()
		{
			//_persistentProgressService.ProgressCreated -= async progress => await HandleProgressCreation(progress);
		}

		/*protected override void Show()
		{
			base.Show();

			foreach (var progressSlot in _progressSlots)
			{
				progressSlot.UpdateView();

				if (progressSlot.Progress == _persistentProgressService.ActiveProgress)
				{
					ChangeSelectedProgressSlot(progressSlot);
				}
			}

			BottomButtonsAreEnabled = (_progressSlots.Count > 0);
		}

		protected override void Hide()
		{
			base.Hide();

			_persistentProgressService.ActiveProgress = _selectedProgressSlot?.Progress;
		}

		protected override void Subscribe()
		{
			base.Subscribe();

			foreach (var progressSlot in _progressSlots)
			{
				progressSlot.Clicked += ChangeSelectedProgressSlot;
			}

			DeleteButton.clicked += HandleDeleteButtonClick;
			RenameButton.clicked += HandleRenameButtonClick;
			LoadButton.clicked += HandleLoadButtonClick;
		}

		protected override void Unsubscribe()
		{
			base.Unsubscribe();

			foreach (var progressSlot in _progressSlots)
			{
				progressSlot.Clicked -= ChangeSelectedProgressSlot;
			}

			DeleteButton.clicked -= HandleDeleteButtonClick;
			RenameButton.clicked -= HandleRenameButtonClick;
			LoadButton.clicked -= HandleLoadButtonClick;
		}

		private async void Initialize()
		{
			_progressSlots = new List<ProgressSlot>(_gameConfig.ProgressSlotsCount);

			foreach (var playerProgress in _persistentProgressService.ProgressSlots.Values)
			{
				var progressSlot = await _templatesFactory.Create<ProgressSlot>(TemplateID.ProgressSlot, ProgressSlotsScrollView);

				progressSlot.Initialize(playerProgress);

				_progressSlots.Add(progressSlot);

				if (playerProgress == _persistentProgressService.ActiveProgress)
				{
					ChangeSelectedProgressSlot(progressSlot);
				}
			}

			_persistentProgressService.ProgressCreated += async progress => await HandleProgressCreation(progress);
		}

		private async UniTask HandleProgressCreation(IPersistentProgress progress)
		{
			var progressSlot = await _templatesFactory.Create<ProgressSlot>(TemplateID.ProgressSlot, ProgressSlotsScrollView);

			progressSlot.Initialize(progress);

			_progressSlots.Add(progressSlot);
		}

		private void HandleDeleteButtonClick()
		{
			var confirmationPopup = PopupsService.Get<ConfirmationPopup>();

			confirmationPopup.Setup("Are you sure", "Cancel", "Delete", null, () =>
			{
				_persistentProgressService.DeleteProgressSlot(_selectedProgressSlot.Progress);
				_progressSlots.Remove(_selectedProgressSlot);
				_selectedProgressSlot.Delete();

				if (_progressSlots.Count > 0)
				{
					var progressSlot = _progressSlots.First();
					ChangeSelectedProgressSlot(progressSlot);

					_persistentProgressService.ActiveProgress = progressSlot.Progress;

					BottomButtonsAreEnabled = true;
				}
				else
				{
					_persistentProgressService.ActiveProgress = null;
					_selectedProgressSlot = null;

					BottomButtonsAreEnabled = false;
				}
			});

			PopupsService.Show<ConfirmationPopup>();
		}

		private void HandleRenameButtonClick()
		{
			_selectedProgressSlot.Rename();
		}

		private void ChangeSelectedProgressSlot(ProgressSlot progressSlot)
		{
			if (progressSlot == _selectedProgressSlot)
			{
				return;
			}

			if (_selectedProgressSlot is not null)
			{
				_selectedProgressSlot.IsSelected = false;
			}

			_selectedProgressSlot = progressSlot;

			if (_selectedProgressSlot is not null)
			{
				_selectedProgressSlot.IsSelected = true;
			}
		}

		private void HandleLoadButtonClick()
		{
			PopupsService.Hide<LoadGamePopup>();
			_screensService.Hide<MainMenuScreen>();

			_gameStateMachine.Enter<LoopsGameState>();
		}*/
	}
}