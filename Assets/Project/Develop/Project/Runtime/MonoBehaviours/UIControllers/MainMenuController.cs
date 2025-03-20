using Runtime.Data.Constants.Enums;
using Runtime.Infrastructure.GameStateMachine.Core;
using Runtime.Infrastructure.GameStateMachine.States;
using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.MonoBehaviours;
using Runtime.Visual.UI.UIDocumentWrappers.Screens;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

public class MainMenuController : InjectedBehaviour
{
	private IGameStateMachine _gameStateMachine;
	private IScreensService _screensService;

	[SerializeField]
	private UIDocument _menuDocument;

	private Button _playButton;
	private Button _settingsButton;
	private Button _exitButton;

	[Inject]
	internal void Construct(IGameStateMachine gameStateMachine, IScreensService screensService)
	{
		_gameStateMachine = gameStateMachine;
		_screensService = screensService;

		Subscribe();
	}

	private void Subscribe()
	{
		if (_menuDocument == null)
		{
			Debug.LogError("UIDocument не назначен!");
			return;
		}

		var root = _menuDocument.rootVisualElement;

		_playButton = root.Q<Button>("PlayButton");
		_settingsButton = root.Q<Button>("SettingsButton");
		_exitButton = root.Q<Button>("ExitButton");

		_playButton.clicked += OnPlayButtonClicked;

		_settingsButton.clicked += OnSettingsButtonClicked;

		_exitButton.clicked += OnExitButtonClicked;
	}

	private void UnSubscribe()
	{
		_playButton.clicked -= OnPlayButtonClicked;

		_settingsButton.clicked -= OnSettingsButtonClicked;

		_exitButton.clicked -= OnExitButtonClicked;
	}

	private void OnDestroy()
	{
		UnSubscribe();
	}

	private void OnPlayButtonClicked()
	{
		_gameStateMachine.Get<LoadingGameState>().SceneName = SceneName.MainMenuSceneAsset;
		_gameStateMachine.Enter<LoadingGameState>();
	}

	private void OnSettingsButtonClicked()
	{
		_screensService.Show<SettingsScreen>();
	}

	private void OnExitButtonClicked()
	{
#if UNITY_EDITOR
		EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
	}
}