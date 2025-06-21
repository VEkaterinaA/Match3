using Runtime.Data.Constants.Enums;
using Runtime.Infrastructure.GameStateMachine.Core;
using Runtime.Infrastructure.GameStateMachine.States;
using Runtime.Infrastructure.Services.Game;
using Runtime.Infrastructure.Services.Game.Core;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;
using Screen = Runtime.Visual.UI.UIDocumentWrappers.Screens.Core.Screen;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens
{
	internal class GameOverScreen : Screen
	{
		private IGameStateMachine _gameStateMachine;
		private IBoardService _boardService;

		private Label Title { get; }
		private Button RestartButton { get; }
		private Button MainMenuButton { get; }

		internal GameOverScreen(UIDocument uiDocument) : base(uiDocument)
		{
			Title = RootVisualElement.Q<Label>(nameof(Title));
			RestartButton = RootVisualElement.Q<Button>(nameof(RestartButton));
			MainMenuButton = RootVisualElement.Q<Button>(nameof(MainMenuButton));
		}

		[Inject]
		internal void Construct(IGameStateMachine gameStateMachine, IBoardService boardService)
		{
			_gameStateMachine = gameStateMachine;
			_boardService = boardService;
		}

		protected override void Show()
		{
			base.Show();
			_gameStateMachine.Enter<PausedGameState>();

			Time.timeScale = 0f;

			_boardService.ResetAll();
		}

		protected override void Hide()
		{
			base.Hide();
			_gameStateMachine.Enter<LoopsGameState>();

			Time.timeScale = 1f;
		}

		protected override void Subscribe()
		{
			base.Subscribe();

/*			RestartButton.clicked += OnRestartButtonClicked;
			MainMenuButton.clicked += OnMainMenuButtonClicked;
*/		}

		protected override void Unsubscribe()
		{
			base.Unsubscribe();

/*			RestartButton.clicked -= OnRestartButtonClicked;
			MainMenuButton.clicked -= OnMainMenuButtonClicked;
*/		}

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
