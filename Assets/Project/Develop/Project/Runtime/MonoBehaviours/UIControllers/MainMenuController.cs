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

		private Button PlayButton;
		private Button ExitButton;
		private Button SettingsButton;
		private Button AchievementsButton;

		[Inject]
		internal void Construct(IGameStateMachine gameStateMachine, IScreensService screensService, IPopupsService popupsService)
		{
			_gameStateMachine = gameStateMachine;
			_screensService = screensService;
			_popupsService = popupsService;
		}

		private void Awake()
		{
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

			if(root == null)
			{
				Debug.LogError("root is null");
			}

			PlayButton = root.Q<Button>(nameof(PlayButton));
			ExitButton = root.Q<Button>(nameof(ExitButton));
			SettingsButton = root.Q<Button>(nameof(SettingsButton));
			AchievementsButton = root.Q<Button>(nameof(AchievementsButton));

			if(PlayButton == null)
			{
				Debug.LogError("PlayButton is null");
			}
			PlayButton.clicked += OnPlayButtonClicked;

			SettingsButton.clicked += OnSettingsButtonClicked;

			AchievementsButton.clicked += OnAchievementsClicked;

			ExitButton.clicked += OnExitButtonClicked;
		}

		private void UnSubscribe()
		{
			PlayButton.clicked -= OnPlayButtonClicked;

			ExitButton.clicked -= OnExitButtonClicked;

			SettingsButton.clicked -= OnSettingsButtonClicked;

			AchievementsButton.clicked -= OnAchievementsClicked;
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