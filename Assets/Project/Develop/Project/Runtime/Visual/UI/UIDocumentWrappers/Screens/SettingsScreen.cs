using Runtime.Data.Progress;
using Runtime.Infrastructure.GameStateMachine.Core;
using Runtime.Infrastructure.GameStateMachine.States;
using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.SaveProgressServices.Core;
using Runtime.Visual.UI.VisualElements.TemplateWrappers.Arrows;
using System;
using UnityEngine.UIElements;
using VContainer;
using Screen = Runtime.Visual.UI.UIDocumentWrappers.Screens.Core.Screen;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens
{
	internal sealed class SettingsScreen : Screen
	{
		private IPersistentProgressService _persistentProgressService;
		private IGameStateMachine _gameStateMachine;
		private IGameService _gameService;

		private SavedSettings SavedSettings => _persistentProgressService.UserInfo.SavedSettings;

		private VisualElement SubtitlesArrowsVisualElement { get; }

		private VisualElement LanguageArrowsVisualElement { get; }

		private VisualElement BackgroundVisualElement { get; }

		private Slider MusicSlider { get; }

		private Slider SoundSlider { get; }

		private Button CloseButton { get; }

		internal SettingsScreen(UIDocument uiDocument) : base(uiDocument)
		{
			SubtitlesArrowsVisualElement = RootVisualElement.Q<VisualElement>(nameof(SubtitlesArrowsVisualElement));
			LanguageArrowsVisualElement = RootVisualElement.Q<VisualElement>(nameof(LanguageArrowsVisualElement));
			BackgroundVisualElement = RootVisualElement.Q<VisualElement>(nameof(BackgroundVisualElement));
			MusicSlider = RootVisualElement.Q<Slider>(nameof(MusicSlider));
			SoundSlider = RootVisualElement.Q<Slider>(nameof(SoundSlider));
			CloseButton = RootVisualElement.Q<Button>(nameof(CloseButton));
		}

		[Inject]
		internal void Construct(IPersistentProgressService persistentProgressService, IGameStateMachine gameStateMachine, IGameService gameService, IObjectResolver objectResolver)
		{
			_persistentProgressService = persistentProgressService;
			_gameStateMachine = gameStateMachine;
			_gameService = gameService;

			objectResolver.Inject(new SubtitlesArrowsButtons(SubtitlesArrowsVisualElement));
			objectResolver.Inject(new LanguageArrowsButtons(LanguageArrowsVisualElement));
		}

		protected override void Initialize()
		{
			base.Initialize();

			MusicSlider.value = SavedSettings.MusicVolumeMultiplier;
			SoundSlider.value = SavedSettings.SoundVolumeMultiplier;
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

			MusicSlider.RegisterValueChangedCallback(UpdateMusicLevel);
			SoundSlider.RegisterValueChangedCallback(UpdateSoundLevel);

			CloseButton.clicked += HideScreen;
		}

		protected override void Unsubscribe()
		{
			base.Unsubscribe();

			MusicSlider.UnregisterValueChangedCallback(UpdateMusicLevel);
			SoundSlider.UnregisterValueChangedCallback(UpdateSoundLevel);

			CloseButton.clicked -= HideScreen;
		}

		private void UpdateMusicLevel(ChangeEvent<Single> musicSliderChangeEvent)
		{
			SavedSettings.MusicVolumeMultiplier = musicSliderChangeEvent.newValue;
		}

		private void UpdateSoundLevel(ChangeEvent<Single> soundSliderChangeEvent)
		{
			SavedSettings.SoundVolumeMultiplier = soundSliderChangeEvent.newValue;
		}

		private void HideScreen()
		{
			ScreensService.Hide<SettingsScreen>();
		}
	}
}