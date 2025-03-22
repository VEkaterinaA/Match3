using Runtime.Data.Progress;
using Runtime.Infrastructure.GameStateMachine.Core;
using Runtime.Infrastructure.GameStateMachine.States;
using Runtime.Infrastructure.Services.Core;
using Runtime.Infrastructure.Services.SaveProgressServices.Core;
using System;
using UnityEngine.UIElements;
using VContainer;
using Screen = Runtime.Visual.UI.UIDocumentWrappers.Screens.Core.Screen;
using Cysharp.Threading.Tasks;
using Runtime.Extensions.System;

namespace Runtime.Visual.UI.UIDocumentWrappers.Screens
{
	internal sealed class SettingsScreen : Screen
	{
		private IPersistentProgressService _persistentProgressService;
		private IGameStateMachine _gameStateMachine;
		private IGameService _gameService;

		private SavedSettings SavedSettings => _persistentProgressService?.UserInfo?.SavedSettings;

		private Slider MusicSlider { get; }

		private Slider SoundSlider { get; }

		private Button CloseButton { get; }

		internal SettingsScreen(UIDocument uiDocument) : base(uiDocument)
		{
			MusicSlider = RootVisualElement.Q<Slider>(nameof(MusicSlider));
			SoundSlider = RootVisualElement.Q<Slider>(nameof(SoundSlider));
			CloseButton = RootVisualElement.Q<Button>(nameof(CloseButton));
		}

		[Inject]
		internal void Construct(IPersistentProgressService persistentProgressService, IGameStateMachine gameStateMachine, IGameService gameService)
		{
			_persistentProgressService = persistentProgressService;
			_gameStateMachine = gameStateMachine;
			_gameService = gameService;

			_persistentProgressService.InvokeAfterInitialization(InitSliders);
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

		private void InitSliders()
		{
			MusicSlider.value = SavedSettings.MusicVolumeMultiplier;
			SoundSlider.value = SavedSettings.SoundVolumeMultiplier;
		}

		private void UpdateMusicLevel(ChangeEvent<Single> musicSliderChangeEvent)
		{
			if (SavedSettings != null)
			{
				SavedSettings.MusicVolumeMultiplier = musicSliderChangeEvent.newValue;
				SaveSettingsAsync().Forget();
			}
		}

		private void UpdateSoundLevel(ChangeEvent<Single> soundSliderChangeEvent)
		{
			if (SavedSettings != null)
			{
				SavedSettings.SoundVolumeMultiplier = soundSliderChangeEvent.newValue;
				SaveSettingsAsync().Forget();
			}
		}

		private void HideScreen()
		{
			if (SavedSettings != null)
			{
				SaveSettingsAsync().Forget();
			}
			ScreensService.Hide<SettingsScreen>();
		}

		private async UniTask SaveSettingsAsync()
		{
			await _persistentProgressService.SaveGameData();
		}
	}
}