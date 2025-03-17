using Runtime.Extensions.LitMotion;
using Runtime.Extensions.System;
using Runtime.Extensions.UnityEngine.UIElements;
using Runtime.Infrastructure.GameStateMachine.Core;
using Runtime.Infrastructure.GameStateMachine.States;
using Runtime.Infrastructure.Services.App.Core;
using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.Visual.UI.UIDocumentWrappers.Popups;
using System;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using Screen = Runtime.Visual.UI.UIDocumentWrappers.Screens.Core.Screen;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens
{
	internal sealed class MainMenuScreen : Screen
	{
		private IApplicationService _applicationService;
		private IGameStateMachine _gameStateMachine;
		private IPopupsService _popupsService;

		private VisualElement SaveIconVisualElement { get; }

		private VisualElement BlurredVisualElement { get; }

		private Button SaveProgressButton { get; }

		private Button ContinueButton { get; }

		private Button LoadGameButton { get; }

		private Button SettingsButton { get; }

		private Button NewGameButton { get; }

		private Button ExitButton { get; }

		internal Boolean IsProgressReset;

		internal MainMenuScreen(UIDocument uiDocument) : base(uiDocument)
		{
			SaveIconVisualElement = RootVisualElement.Q<VisualElement>(nameof(SaveIconVisualElement));
			BlurredVisualElement = RootVisualElement.Q<VisualElement>(nameof(BlurredVisualElement));
			SaveProgressButton = RootVisualElement.Q<Button>(nameof(SaveProgressButton));
			ContinueButton = RootVisualElement.Q<Button>(nameof(ContinueButton));
			LoadGameButton = RootVisualElement.Q<Button>(nameof(LoadGameButton));
			SettingsButton = RootVisualElement.Q<Button>(nameof(SettingsButton));
			NewGameButton = RootVisualElement.Q<Button>(nameof(NewGameButton));
			ExitButton = RootVisualElement.Q<Button>(nameof(ExitButton));
		}

		[Inject]
		internal void Construct(IApplicationService applicationService, IGameStateMachine gameStateMachine, IPopupsService popupsService)
		{
			_applicationService = applicationService;
			_gameStateMachine = gameStateMachine;
			_popupsService = popupsService;
		}

		internal void PlaySaveAnimation()
		{
			var builder = SaveIconVisualElement.style.CreateMotion(0.0F, 1.0F, 0.2F);
			builder.InvokeAfterCompletion(() => SaveIconVisualElement.style.CreateMotion(1.0F, 0.0F, 2.8F).BindToOpacity());
			builder.BindToOpacity();

			SaveIconVisualElement.style.CreateMotion(80.0F, 300.0F, 3.0F).BindToBottom();
		}

		protected override void Show()
		{
			base.Show();

			_gameStateMachine.Enter<PausedGameState>();

			UpdateView();
		}

		protected override void Hide()
		{
			base.Hide();

			_gameStateMachine.Enter<LoopsGameState>();
		}

		protected override void Subscribe()
		{
			base.Subscribe();

			LoadGameButton.clicked += _popupsService.Show<LoadGamePopup>;
			_popupsService.Get<LoadGamePopup>().Hided += UpdateView;

			SettingsButton.clicked += ShowSettingsScreen;
			ExitButton.clicked += ExitGame;
		}

		protected override void Unsubscribe()
		{
			base.Unsubscribe();

			LoadGameButton.clicked -= _popupsService.Show<LoadGamePopup>;
			_popupsService.Get<LoadGamePopup>().Hided -= UpdateView;

			SettingsButton.clicked -= ShowSettingsScreen;
			ExitButton.clicked -= ExitGame;
		}

		private void UpdateView()
		{
			IsProgressReset = ContinueButton.style.display == DisplayStyle.None;
		}

		private void ShowSettingsScreen()
		{
			ScreensService.Show<SettingsScreen>();
		}

		private void ExitGame()
		{
			_applicationService.RequestQuit();
		}
	}
}