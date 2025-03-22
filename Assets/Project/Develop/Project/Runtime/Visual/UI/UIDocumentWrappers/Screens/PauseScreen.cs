using Runtime.Data.Constants.Enums;
using Runtime.Infrastructure.GameStateMachine.Core;
using Runtime.Infrastructure.GameStateMachine.States;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using Screen = Runtime.Visual.UI.UIDocumentWrappers.Screens.Core.Screen;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens
{
	internal sealed class PauseScreen : Screen
	{
		private IGameStateMachine _gameStateMachine;

		private Button ResumeButton { get; }
		private Button RestartButton { get; }
		private Button MainMenuButton { get; }

		internal PauseScreen(UIDocument uiDocument) : base(uiDocument)
		{
			ResumeButton = RootVisualElement.Q<Button>(nameof(ResumeButton));
			RestartButton = RootVisualElement.Q<Button>(nameof(RestartButton));
			MainMenuButton = RootVisualElement.Q<Button>(nameof(MainMenuButton));
		}

		[Inject]
		internal void Construct(IGameStateMachine gameStateMachine)
		{
			_gameStateMachine = gameStateMachine;
		}

		protected override void Show()
		{
			base.Show();
			_gameStateMachine.Enter<PausedGameState>();
		}

		protected override void Hide()
		{
			base.Hide();
			_gameStateMachine.Enter<LoopsGameState>();
		}

		protected override void Subscribe()
		{
			base.Subscribe();

			ResumeButton.clicked += OnResumeButtonClicked;
			RestartButton.clicked += OnRestartButtonClicked;
			MainMenuButton.clicked += OnMainMenuButtonClicked;
		}

		protected override void Unsubscribe()
		{
			base.Unsubscribe();

			ResumeButton.clicked -= OnResumeButtonClicked;
			RestartButton.clicked -= OnRestartButtonClicked;
			MainMenuButton.clicked -= OnMainMenuButtonClicked;
		}

		private void OnResumeButtonClicked()
		{
			ScreensService.Hide<PauseScreen>();
		}

		private void OnRestartButtonClicked()
		{
			_gameStateMachine.Get<LoadingGameState>().SceneName = SceneName.CoreSceneAsset;
			_gameStateMachine.Enter<LoadingGameState>();
			ScreensService.Hide<PauseScreen>();
		}

		private void OnMainMenuButtonClicked()
		{
			_gameStateMachine.Get<LoadingGameState>().SceneName = SceneName.MainMenuSceneAsset;
			_gameStateMachine.Enter<LoadingGameState>();
			ScreensService.Hide<PauseScreen>();
		}
	}
}