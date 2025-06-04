using System;
using UnityEngine;

namespace Runtime.Data.Progress
{
	[Serializable]
	internal class Settings
	{
		[SerializeField]
		[Range(0.0f, 1.0f)]
		private Single _musicVolumeMultiplier;
		[SerializeField]
		[Range(0.0f, 1.0f)]
		private Single _soundVolumeMultiplier;

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
