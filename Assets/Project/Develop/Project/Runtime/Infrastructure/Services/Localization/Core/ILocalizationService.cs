using System;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;

namespace Runtime.Infrastructure.Services.Localization.Core
{
	internal interface ILocalizationService
	{
		internal event Action LocaleChanged;

		internal LocalizedStringDatabase StringDatabase { get; }

		internal ILocalesProvider AvailableLocales { get; }

		internal Int32 SubtitlesLocaleIndex { get; set; }

		internal Int32 SelectedLocaleIndex { get; set; }

		internal Locale SubtitlesLocale { get; set; }

		internal Locale LanguageLocale { get; set; }

		internal void GetLocalizedString(Locale locale, String localizationKey, Action<String> onComplete, Action onFail = null);

		internal void GetLocalizedString(String localizationKey, Action<String> onComplete, Action onFail = null);
	}
}
