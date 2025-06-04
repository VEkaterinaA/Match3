using Runtime.Data.Constants.Enums;
using Runtime.Infrastructure.GameStateMachine.Core;
using Runtime.Infrastructure.GameStateMachine.States;
using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.UIServices;
using Runtime.Infrastructure.Services.UIServices.Core;
using Runtime.MonoBehaviours;
using Runtime.Visual.UI.UIDocumentWrappers.Popups;
using Runtime.Visual.UI.UIDocumentWrappers.Screens;
using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using VContainer;

namespace Runtime.MonoBehaviours.UIControllers
{
	public class MainMenuController : InjectedBehaviour
	{
		private IGameStateMachine _gameStateMachine;
		private IScreensService _screensService;
		private IPopupsService _popupsService;

		[SerializeField]
		private UIDocument _menuDocument;

		private Button _playButton;
		private Button _settingsButton;
		private Button _achievementsButton;
		private Button _exitButton;

		[Inject]
		internal void Construct(IGameStateMachine gameStateMachine, IScreensService screensService, IPopupsService popupsService)
		{
			_gameStateMachine = gameStateMachine;
			_screensService = screensService;
			_popupsService = popupsService;

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
			_exitButton = root.Q<Button>("ExitButton");
			_settingsButton = root.Q<Button>("SettingsButton");
			_achievementsButton = root.Q<Button>("AchievementsButton");

			_playButton.clicked += OnPlayButtonClicked;

			_settingsButton.clicked += OnSettingsButtonClicked;

			_achievementsButton.clicked += OnAchievementsClicked;

			_exitButton.clicked += OnExitButtonClicked;
		}

		private void UnSubscribe()
		{
			_playButton.clicked -= OnPlayButtonClicked;

			_exitButton.clicked -= OnExitButtonClicked;

			_settingsButton.clicked -= OnSettingsButtonClicked;

			_achievementsButton.clicked -= OnAchievementsClicked;
		}

		private void OnDestroy()
		{
			UnSubscribe();
		}

		private void OnPlayButtonClicked()
		{
			_popupsService.Show<LevelEditorPopup>();
		}

		private void OnSettingsButtonClicked()
		{
			_screensService.Show<SettingsScreen>();
		}

		private void OnAchievementsClicked()
		{

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
}