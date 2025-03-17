using System;
using UnityEngine;

namespace Runtime.Data.Progress
{
	[Serializable]
	internal sealed class SavedSettings
	{
		[SerializeField] [Range(0.0f, 1.0f)]
		private Single _musicVolumeMultiplier;
		[SerializeField] [Range(0.0f, 1.0f)]
		private Single _soundVolumeMultiplier;
		[SerializeField]
		private Boolean _subtitlesAreEnabled;
		[SerializeField]
		private Int32 _subtitlesLocaleIndex;
		[SerializeField]
		private Int32 _languageLocaleIndex;

		internal Boolean SubtitlesIsEnabled
		{
			get => _subtitlesAreEnabled;
			set => _subtitlesAreEnabled = value;
		}

		internal Int32 LanguageLocaleIndex
		{
			get => _languageLocaleIndex;
			set => _languageLocaleIndex = value;
		}

		internal Int32 SubtitlesLocaleIndex
		{
			get => _subtitlesLocaleIndex;
			set => _subtitlesLocaleIndex = value;
		}

		internal Single MusicVolumeMultiplier
		{
			get => _musicVolumeMultiplier;
			set => _musicVolumeMultiplier = value;
		}

		internal Single SoundVolumeMultiplier
		{
			get => _soundVolumeMultiplier;
			set => _soundVolumeMultiplier = value;
		}
	}
}