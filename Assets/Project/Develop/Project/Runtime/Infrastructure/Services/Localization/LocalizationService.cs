using Cysharp.Threading.Tasks;
using Runtime.Data.Progress;
using Runtime.Extensions.System;
using Runtime.Infrastructure.Core;
using Runtime.Infrastructure.Services.Localization.Core;
using Runtime.Infrastructure.Services.SaveProgressServices.Core;
using System;
using System.Globalization;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
using VContainer;

namespace Runtime.Infrastructure.Services.Localization
{
	internal sealed class LocalizationService : ILocalizationService, IInitializationInformer, IDisposable
	{
		private IPersistentProgressService _persistentProgressService;

		private LocalizationSettings _localizationSettings;

		private Action _localeChanged;
		private Action _initialized;

		private Boolean _isInitialized;

		event Action ILocalizationService.LocaleChanged
		{
			add => _localeChanged += value;
			remove => _localeChanged -= value;
		}

		event Action IInitializationInformer.Initialized
		{
			add => _initialized += value;
			remove => _initialized -= value;
		}

		LocalizedStringDatabase ILocalizationService.StringDatabase => _localizationSettings.GetStringDatabase();

		ILocalesProvider ILocalizationService.AvailableLocales => _localizationSettings.GetAvailableLocales();

		Boolean IInitializationInformer.IsInitialized => _isInitialized;

		Int32 ILocalizationService.SelectedLocaleIndex
		{
			get => SavedSettings.LanguageLocaleIndex;
			set
			{
				var localesCount = Service.AvailableLocales.Locales.Count;
				SavedSettings.LanguageLocaleIndex = ((value + localesCount) % localesCount);

				UpdateLocale();
			}
		}

		Locale ILocalizationService.SubtitlesLocale { get; set; }

		Int32 ILocalizationService.SubtitlesLocaleIndex
		{
			get => SavedSettings.SubtitlesLocaleIndex;
			set
			{
				var localesCount = Service.AvailableLocales.Locales.Count;
				SavedSettings.SubtitlesLocaleIndex = ((value + localesCount) % localesCount);

				UpdateSubtitlesLocale();
			}
		}

		Locale ILocalizationService.LanguageLocale
		{
			get => _localizationSettings.GetSelectedLocale();
			set => _localizationSettings.SetSelectedLocale(value);
		}

		private SavedSettings SavedSettings => _persistentProgressService.UserInfo.SavedSettings;

		private ILocalizationService Service => this;

		[Inject]
		internal async void Construct(IPersistentProgressService persistentProgressService)
		{
			_persistentProgressService = persistentProgressService;

			_localizationSettings = await LocalizationSettings.InitializationOperation.ToUniTask();

			if (SavedSettings.LanguageLocaleIndex == -1)
			{
				SavedSettings.LanguageLocaleIndex = Service.AvailableLocales.Locales.IndexOf(Service.AvailableLocales.GetLocale(CultureInfo.CurrentCulture));
			}

			Subscribe();

			UpdateSubtitlesLocale();
			UpdateLocale();

			_isInitialized = true;
			_initialized?.Invoke();
		}

		void IDisposable.Dispose()
		{
			Unsubscribe();
		}

		async void ILocalizationService.GetLocalizedString(Locale locale, String localizationKey, Action<String> onComplete, Action onFail)
		{
			await UniTask.WaitUntil(() => _isInitialized);

			var localizedString = await Service.StringDatabase.GetLocalizedStringAsync(localizationKey, locale);

			if (localizedString.IsNullOrEmpty())
			{
				onFail?.Invoke();
			}
			else
			{
				onComplete.Invoke(localizedString);
			}
		}

		async void ILocalizationService.GetLocalizedString(String localizationKey, Action<String> onComplete, Action onFail)
		{
			await UniTask.WaitUntil(() => _isInitialized);

			var localizedString = await Service.StringDatabase.GetLocalizedStringAsync(localizationKey);

			if (localizedString.IsNullOrEmpty())
			{
				onFail?.Invoke();
			}
			else
			{
				onComplete.Invoke(localizedString);
			}
		}

		private void UpdateLocale()
		{
			Service.LanguageLocale = Service.AvailableLocales.Locales[SavedSettings.LanguageLocaleIndex];
		}

		private void UpdateSubtitlesLocale()
		{
			Service.SubtitlesLocale = Service.AvailableLocales.Locales[SavedSettings.SubtitlesLocaleIndex];
		}

		private void HandleLocalChange(Locale locale)
		{
			_localeChanged?.Invoke();
		}

		private void Subscribe()
		{
			_localizationSettings.OnSelectedLocaleChanged += HandleLocalChange;
		}

		private void Unsubscribe()
		{
			_localizationSettings.OnSelectedLocaleChanged -= HandleLocalChange;
		}
	}
}
